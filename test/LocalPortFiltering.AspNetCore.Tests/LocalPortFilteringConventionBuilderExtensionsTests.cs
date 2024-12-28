using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LocalPortFiltering.AspNetCore.Tests;
public class LocalPortFilteringConventionBuilderExtensionsTests
{
    [Fact]
    public void RequireLocalPort_AllowPort_MetadataAdded()
    {
        // Arrange
        var testConventionBuilder = new TestEndpointConventionBuilder();

        // Act
        testConventionBuilder.RequireLocalPortFiltering([80, 443]);

        // Assert
        var addCorsPolicy = Assert.Single(testConventionBuilder.Conventions);

        var endpointModel = new TestEndpointBuilder();
        addCorsPolicy(endpointModel);
        var endpoint = endpointModel.Build();

        var metadata = endpoint.Metadata.GetMetadata<ILocalPortFilteringData>();
        Assert.NotNull(metadata);
        Assert.Contains(80, metadata.AllowPorts);
        Assert.Contains(443, metadata.AllowPorts);
    }

    [Fact]
    public void RequireLocalPort_ChainedCall_ReturnedBuilderIsDerivedType()
    {
        // Arrange
        var testConventionBuilder = new TestEndpointConventionBuilder();

        // Act
        var builder = testConventionBuilder.RequireLocalPortFiltering(80);

        // Assert
        Assert.True(builder.TestProperty);
    }

    private class TestEndpointBuilder : EndpointBuilder
    {
        public override Endpoint Build()
        {
            return new Endpoint(RequestDelegate, new EndpointMetadataCollection(Metadata), DisplayName);
        }
    }

    private class TestEndpointConventionBuilder : IEndpointConventionBuilder
    {
        public IList<Action<EndpointBuilder>> Conventions { get; } = new List<Action<EndpointBuilder>>();
        public bool TestProperty { get; } = true;

        public void Add(Action<EndpointBuilder> convention)
        {
            Conventions.Add(convention);
        }
    }
}
