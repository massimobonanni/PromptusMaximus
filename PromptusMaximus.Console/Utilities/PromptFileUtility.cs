using PromptusMaximus.Core.Configuration;

namespace PromptusMaximus.Console.Utilities;

internal static class PromptFileUtility
{
    private static readonly string PromptsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prompts");

    /// <summary>
    /// Retrieves the system prompt text for the specified language.
    /// </summary>
    /// <param name="language">The language for which to retrieve the system prompt.</param>
    /// <returns>The system prompt text, or null if the file doesn't exist or cannot be read.</returns>
    public static async Task<string?> GetSystemPromptAsync(Languages language)
    {
        var fileName = $"SystemPrompt_{language}.txt";
        var filePath = Path.Combine(PromptsDirectory, fileName);

        try
        {
            if (!File.Exists(filePath))
            {
                ConsoleUtility.WriteLine($"System prompt file not found: {filePath}", ConsoleColor.Yellow);
                return null;
            }

            return await File.ReadAllTextAsync(filePath);
        }
        catch (Exception ex)
        {
            ConsoleUtility.WriteLine($"Error reading system prompt file '{filePath}': {ex.Message}", ConsoleColor.Red);
            return null;
        }
    }

    /// <summary>
    /// Retrieves the system prompt text for the specified language synchronously.
    /// </summary>
    /// <param name="language">The language for which to retrieve the system prompt.</param>
    /// <returns>The system prompt text, or null if the file doesn't exist or cannot be read.</returns>
    public static string? GetSystemPrompt(Languages language)
    {
        var fileName = $"SystemPrompt_{language}.txt";
        var filePath = Path.Combine(PromptsDirectory, fileName);

        try
        {
            if (!File.Exists(filePath))
            {
                ConsoleUtility.WriteLine($"System prompt file not found: {filePath}", ConsoleColor.Yellow);
                return null;
            }

            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            ConsoleUtility.WriteLine($"Error reading system prompt file '{filePath}': {ex.Message}", ConsoleColor.Red);
            return null;
        }
    }

    /// <summary>
    /// Checks if a system prompt file exists for the specified language.
    /// </summary>
    /// <param name="language">The language to check.</param>
    /// <returns>True if the file exists, false otherwise.</returns>
    public static bool SystemPromptExists(Languages language)
    {
        var fileName = $"SystemPrompt_{language}.txt";
        var filePath = Path.Combine(PromptsDirectory, fileName);
        return File.Exists(filePath);
    }

    /// <summary>
    /// Gets the full file path for a system prompt file for the specified language.
    /// </summary>
    /// <param name="language">The language for which to get the file path.</param>
    /// <returns>The full file path to the system prompt file.</returns>
    public static string GetSystemPromptFilePath(Languages language)
    {
        var fileName = $"SystemPrompt_{language}.txt";
        return Path.Combine(PromptsDirectory, fileName);
    }
}