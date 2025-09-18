using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromptusMaximus.Console.Utilities;

namespace GitHubModel.Client.Models;

internal static class GitHubModelExtensions
{
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
