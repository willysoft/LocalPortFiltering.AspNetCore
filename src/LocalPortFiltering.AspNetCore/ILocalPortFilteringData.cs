namespace LocalPortFiltering.AspNetCore;

/// <summary>
/// Represents the interface for local port filtering data.
/// </summary>
public interface ILocalPortFilteringData
{
    /// <summary>
    /// Gets the list of port numbers that are allowed through the filter.
    /// </summary>
    IReadOnlyList<int> AllowPorts { get; }
}

