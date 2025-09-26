namespace PromptusMaximus.Core.Models;

/// <summary>
/// Represents a GitHub model with its properties and capabilities.
/// </summary>
public class GitHubModel
{
    /// <summary>
    /// The unique identifier for the model.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The name of the model.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The registry where the model is listed.
    /// </summary>
    public string? Registry { get; set; }

    /// <summary>
    /// The publisher of the model.
    /// </summary>
    public string? Publisher { get; set; }

    /// <summary>
    /// A brief summary of the model's capabilities.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// The rate limit tier for the model.
    /// </summary>
    public string? RateLimitTier { get; set; }

    /// <summary>
    /// The URL to the model's detail page.
    /// </summary>
    public string? HtmlUrl { get; set; }

    /// <summary>
    /// The version of the model.
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// A list of capabilities supported by the model.
    /// </summary>
    public List<string> Capabilities { get; set; } = new();

    /// <summary>
    /// The limits for the model, including input/output token limits.
    /// </summary>
    public GitHubModelLimits? Limits { get; set; }

    /// <summary>
    /// A list of tags associated with the model.
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// A list of input modalities supported by the model.
    /// </summary>
    public List<string> SupportedInputModalities { get; set; } = new();

    /// <summary>
    /// A list of output modalities supported by the model.
    /// </summary>
    public List<string> SupportedOutputModalities { get; set; } = new();
}