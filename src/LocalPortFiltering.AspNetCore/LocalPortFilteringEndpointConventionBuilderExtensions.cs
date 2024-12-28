using LocalPortFiltering.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Authorization extension methods for <see cref="IEndpointConventionBuilder"/>.
/// </summary>
public static class LocalPortFilteringEndpointConventionBuilderExtensions
{
    /// <summary>
    /// Adds a requirement for local port filtering to the endpoint.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder.</typeparam>
    /// <param name="builder">The endpoint convention builder instance.</param>
    /// <param name="allowPorts">The allowed ports for local port filtering.</param>
    /// <returns>The updated endpoint convention builder.</returns>
    public static TBuilder RequireLocalPortFiltering<TBuilder>(this TBuilder builder, params int[] allowPorts)
        where TBuilder : IEndpointConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);

        RequireLocalPortFilteringCore(builder, new LocalPortFilteringAttribute(allowPorts));

        return builder;
    }

    private static void RequireLocalPortFilteringCore<TBuilder>(TBuilder builder, ILocalPortFilteringData filteringData)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(endpointBuilder =>
        {
            endpointBuilder.Metadata.Add(filteringData);
        });
    }
}
