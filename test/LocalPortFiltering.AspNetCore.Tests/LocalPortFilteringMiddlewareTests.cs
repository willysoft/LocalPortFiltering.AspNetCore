using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;

namespace LocalPortFiltering.AspNetCore.Tests;

public class LocalPortFilteringMiddlewareTests
{
    [Fact]
    public async Task TestLocalPortFiltering_DefaultBehavior()
    {
        // Arrange
        using var host = new HostBuilder()
            .ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder.UseTestServer()
                              .ConfigureServices(services =>
                              {
                                  services.AddLocalPortFiltering();
                              })
                              .Configure(app =>
                              {
                                  app.UseLocalPortFiltering();
                                  app.Run(c => Task.CompletedTask);
                              });
            })
            .Build();
        await host.StartAsync();
        using var server = host.GetTestServer();

        // Act
        var response = await server.CreateClient().GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData(200, 443, new int[] { 443 })]
    [InlineData(403, 80, new int[] { 443 })]
    [InlineData(200, 3000, new int[] { 3000, 3001 })]
    [InlineData(403, 4000, new int[] { 3000, 3001 })]
    [InlineData(200, 1005, new int[] { 1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010 })]
    [InlineData(403, 1500, new int[] { 1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010 })]
    public async Task TestLocalPortFiltering_RejectsBlockedPort(int status, int localPort, int[] allowPort)
    {
        // Arrange
        var options = new LocalPortFilteringOptions();
        var optionsMonitor = new Mock<IOptionsMonitor<LocalPortFilteringOptions>>();
        optionsMonitor.Setup(o => o.CurrentValue)
                      .Returns(options)
                      .Verifiable();
        var middleware = new LocalPortFilteringMiddleware(Mock.Of<RequestDelegate>(), optionsMonitor.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.SetEndpoint(new Endpoint(c => Task.CompletedTask, new EndpointMetadataCollection(new LocalPortFilteringAttribute(allowPort)), "Test endpoint"));
        httpContext.Connection.LocalPort = localPort;

        // Act
        await middleware.Invoke(httpContext);

        // Assert
        Assert.Equal(status, (int)httpContext.Response.StatusCode);
    }
}