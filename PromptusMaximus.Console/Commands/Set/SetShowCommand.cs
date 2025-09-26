using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

/// <summary>
/// Command that displays the current configuration settings for the session.
/// Shows the default model, language preference, and GitHub token status.
/// </summary>
internal class SetShowCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetShowCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager used to access current settings and tokens.</param>
    public SetShowCommand(ISessionManager sessionManager) :
        base("show", "Show current settings", sessionManager)
    {
        this.SetAction(CommandHandler);
    }

    /// <summary>
    /// Handles the execution of the show command by displaying current session settings.
    /// Loads the latest settings and outputs the model, language, and GitHub token status to the console.
    /// </summary>
    /// <param name="parseResult">The parsed command line arguments (not used in this implementation).</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
    {
        await this._sessionManager.LoadSettingsAsync();
        System.Console.WriteLine($"Default model : {this._sessionManager.CurrentSettings.Model}");
        System.Console.WriteLine($"Default language : {this._sessionManager.CurrentSettings.Language}");

        var token = this._sessionManager.GetGitHubToken();

        if (string.IsNullOrEmpty(token))
        {
            System.Console.WriteLine("GitHub Token : Not Set");
        }
        else
        {
            System.Console.WriteLine($"GitHub Token : {token.Mask()}");
        }
    }
}
