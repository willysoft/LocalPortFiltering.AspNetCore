namespace LocalPortFiltering.AspNetCore;

/// <inheritdoc />
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class LocalPortFilteringAttribute : Attribute, ILocalPortFilteringData
{
    /// <inheritdoc />
    public IReadOnlyList<int> AllowPorts { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalPortFilteringAttribute"/> class.
    /// </summary>
    /// <param name="allowPort">The port number that is allowed through the filter.</param>
    public LocalPortFilteringAttribute(int allowPort)
         : this([allowPort])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalPortFilteringAttribute"/> class.
    /// </summary>
    /// <param name="allowPorts">The port numbers that is allowed through the filter.</param>
    public LocalPortFilteringAttribute(params int[] allowPorts)
    {
        ArgumentNullException.ThrowIfNull(allowPorts);

        AllowPorts = allowPorts.ToArray();
    }
}
