using LocalPortFiltering.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extension methods for the host filtering middleware.
/// </summary>
public static class LocalPortFilteringServicesExtensions
{
    /// <summary>
    /// Adds the local port filtering services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddLocalPortFiltering(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services.AddLocalPortFiltering(options => { });
    }

    /// <summary>
    /// Adds the local port filtering services to the service collection with custom configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">An action to configure the <see cref="LocalPortFilteringOptions"/>.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddLocalPortFiltering(this IServiceCollection services, Action<LocalPortFilteringOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.Configure(configureOptions);
        return services;
    }
}
