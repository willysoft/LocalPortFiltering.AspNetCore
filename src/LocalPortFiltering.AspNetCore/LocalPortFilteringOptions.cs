namespace LocalPortFiltering.AspNetCore;

public class LocalPortFilteringOptions
{
    /// <summary>
    /// Indicates if the 403 response should include a default message or be empty. This is enabled by default.
    /// </summary>
    public bool IncludeFailureMessage { get; set; } = true;
}
