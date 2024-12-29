using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text;

namespace LocalPortFiltering.AspNetCore;

/// <summary>
/// Middleware to handle local port filtering based on the specified options.
/// </summary>
public class LocalPortFilteringMiddleware
{
    // Matches Http.Sys.
    internal static readonly byte[] DefaultResponse = Encoding.ASCII.GetBytes(
            "{\"message\":\"Access to the requested port is not allowed.\"}"
    );

    private readonly RequestDelegate m_Next;
    private readonly IOptionsMonitor<LocalPortFilteringOptions> m_OptionsMonitor;

    private LocalPortFilteringOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalPortFilteringMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the request pipeline.</param>
    /// <param name="optionsMonitor">The options monitor to manage <see cref="LocalPortFilteringOptions"/>.</param>
    public LocalPortFilteringMiddleware(RequestDelegate next, IOptionsMonitor<LocalPortFilteringOptions> optionsMonitor)
    {
        m_Next = next ?? throw new ArgumentNullException(nameof(next));
        m_OptionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));

        _options = optionsMonitor.CurrentValue;
        m_OptionsMonitor.OnChange(options =>
        {
            _options = options;
        });
    }

    /// <summary>
    /// Handles the HTTP request and applies local port filtering.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the completion of the request handling.</returns>
    public Task Invoke(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var endpoint = context.GetEndpoint();
        var exceptionHandler = endpoint?.Metadata.GetMetadata<ILocalPortFilteringData>();
        if (exceptionHandler == null)
            return m_Next(context);

        if (!exceptionHandler.AllowPorts.Contains(context.Connection.LocalPort))
            return HostValidationFailed(context);

        return m_Next(context);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private Task HostValidationFailed(HttpContext context)
    {
        context.Response.StatusCode = 403;
        if (_options.IncludeFailureMessage)
        {
            context.Response.ContentLength = DefaultResponse.Length;
            context.Response.ContentType = "application/json";
            return context.Response.Body.WriteAsync(DefaultResponse, 0, DefaultResponse.Length);
        }
        return Task.CompletedTask;
    }
}