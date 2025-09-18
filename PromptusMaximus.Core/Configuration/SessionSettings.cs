using System.Text.Json.Serialization;

namespace PromptusMaximus.Core.Configuration;

public class SessionSettings
{
    public string? Model { get; set; }
    public string? Language { get; set; } = "en";
    public Dictionary<string, string> CustomSettings { get; set; } = new();
    
    [JsonIgnore]
    public Dictionary<string, string> Secrets { get; set; } = new();
}