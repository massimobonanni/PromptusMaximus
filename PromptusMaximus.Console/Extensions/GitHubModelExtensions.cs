using PromptusMaximus.Console.Utilities;

namespace PromptusMaximus.Core.Models;

/// <summary>
/// Extension methods for the GitHubModel class to provide display functionality.
/// </summary>
internal static class GitHubModelExtensions
{
    /// <summary>
    /// Displays the GitHub model information in a formatted console output.
    /// </summary>
    /// <param name="model">The GitHub model to display.</param>
    public static void Display(this GitHubModel model)
    {
        ConsoleUtility.WriteLine($"Model: {model.Name}", ConsoleColor.Green);
        ConsoleUtility.WriteLine($"\tId: {model.Id}");
        ConsoleUtility.WriteLine($"\tSummary: {model.Summary}");
        ConsoleUtility.WriteLine($"\tPublisher: {model.Publisher}");
        ConsoleUtility.WriteLine($"\tVersion: {model.Version}");
        ConsoleUtility.WriteLine($"\tPage: {model.HtmlUrl}");
        ConsoleUtility.WriteLine($"\tCapabilities: {string.Join(", ", model.Capabilities)}");
    }
}
