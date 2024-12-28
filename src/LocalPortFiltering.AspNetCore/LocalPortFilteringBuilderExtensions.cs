using LocalPortFiltering.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for the LocalPortFiltering middleware.
/// </summary>
public static class LocalPortFilteringBuilderExtensions
{
    /// <summary>
    /// Adds the LocalPortFiltering middleware to the application's request pipeline.
    /// </summary>
    /// <param name="app">The application's IApplicationBuilder instance.</param>
    /// <returns>The updated IApplicationBuilder instance.</returns>
    public static IApplicationBuilder UseLocalPortFiltering(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseMiddleware<LocalPortFilteringMiddleware>();

        return app;
    }
}
