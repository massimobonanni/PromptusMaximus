namespace GitHubModel.Client.Models;

/// <summary>
/// Represents the limits for a GitHub model, including input/output token limits.
/// </summary>
public class GitHubModelLimits
{
    /// <summary>
    /// The maximum number of input tokens allowed.
    /// </summary>
    public int? MaxInputTokens { get; set; }

    /// <summary>
    /// The maximum number of output tokens allowed.
    /// </summary>
    public int? MaxOutputTokens { get; set; }
}