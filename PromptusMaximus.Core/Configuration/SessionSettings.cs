using System.Text.Json.Serialization;

namespace PromptusMaximus.Core.Configuration;

public class SessionSettings
{
    public string? Model { get; set; }
    public Languages Language { get; set; } = Languages.en;
    public Dictionary<string, string> CustomSettings { get; set; } = new();
    
    [JsonIgnore]
    public Dictionary<string, string> Secrets { get; set; } = new();
}