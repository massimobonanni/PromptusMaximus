using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Console.Services;
using PromptusMaximus.Console.Utilities;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;
using System.Reflection;

namespace PromptusMaximus.Console.Commands;

/// <summary>
/// Implements a command that displays version information for the PromptusMaximus console application.
/// This command retrieves and displays assembly version details including the main version and file version.
/// </summary>
internal class VersionCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VersionCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager instance used to handle session-related operations.</param>
    public VersionCommand(ISessionManager sessionManager) :
        base("version", "Show the version of the console", sessionManager)
    {
        this.SetAction(CommandHandler);
    }

    /// <summary>
    /// Handles the execution of the version command by loading session settings and displaying version information.
    /// Retrieves assembly version details and outputs them to the console in green text.
    /// </summary>
    /// <param name="parseResult">The parsed command-line arguments and options.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous command execution.</returns>
    private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
    {
        await this._sessionManager.LoadSettingsAsync();

        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version?.ToString();
        var fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        var informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        ConsoleUtility.WriteLine($"Version: {version}", ConsoleColor.Green);
        ConsoleUtility.WriteLine($"File Version: {fileVersion}", ConsoleColor.Green);
    }
}
