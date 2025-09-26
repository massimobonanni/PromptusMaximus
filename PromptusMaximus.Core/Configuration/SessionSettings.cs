using System.Text.Json.Serialization;

namespace PromptusMaximus.Core.Configuration;

/// <summary>
/// Represents configuration settings for a user session, including model selection, language preferences, and custom settings.
/// </summary>
public class SessionSettings
{
    /// <summary>
    /// Gets or sets the AI model to be used for the session.
    /// </summary>
    /// <value>The name or identifier of the AI model, or null if not specified.</value>
    public string? Model { get; set; }

    /// <summary>
    /// Gets or sets the language preference for the session.
    /// </summary>
    /// <value>The selected language from the <see cref="Languages"/> enumeration. Defaults to <see cref="Languages.en"/>.</value>
    public Languages Language { get; set; } = Languages.en;

    /// <summary>
    /// Gets or sets a collection of custom configuration settings specific to the session.
    /// </summary>
    /// <value>A dictionary containing key-value pairs of custom settings. Initialized as an empty dictionary.</value>
    public Dictionary<string, string> CustomSettings { get; set; } = new();
    
    /// <summary>
    /// Gets or sets a collection of sensitive configuration values that should not be serialized.
    /// </summary>
    /// <value>A dictionary containing key-value pairs of secret values such as API keys or tokens. This property is excluded from JSON serialization.</value>
    [JsonIgnore]
    public Dictionary<string, string> Secrets { get; set; } = new();
}