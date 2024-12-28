namespace LocalPortFiltering.AspNetCore;

/// <summary>
/// Represents the interface for local port filtering data.
/// </summary>
public interface ILocalPortFilteringData
{
    /// <summary>
    /// Gets the port number that is allowed through the filter.
    /// </summary>
    int AllowPort { get; }
}

