namespace LocalPortFiltering.AspNetCore;

/// <inheritdoc />
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class LocalPortFilteringAttribute : Attribute, ILocalPortFilteringData
{
    /// <inheritdoc />
    public int AllowPort { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalPortFilteringAttribute"/> class.
    /// </summary>
    /// <param name="allowPort">The port number that is allowed through the filter.</param>
    public LocalPortFilteringAttribute(int allowPort)
    {
        AllowPort = allowPort;
    }
}
