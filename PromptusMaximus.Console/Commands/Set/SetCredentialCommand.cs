using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

/// <summary>
/// Command for setting authentication credentials, specifically GitHub tokens.
/// </summary>
internal class SetCredentialCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetCredentialCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager used to store and manage authentication credentials.</param>
    public SetCredentialCommand(ISessionManager sessionManager) :
        base("credential", "Set authentication credentials", sessionManager)
    {
        var tokenOption = new Option<string>(name: "--token")
        {
            Description = "GitHub token for authentication",
            Required = true,
        };
        tokenOption.Aliases.Add("-t");
        this.Options.Add(tokenOption);

        this.SetAction(CommandHandler);
    }

    /// <summary>
    /// Handles the execution of the set credential command.
    /// </summary>
    /// <param name="parseResult">The parsed command line arguments containing the token value.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous command execution operation.</returns>
    private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var ghToken = parseResult.GetValue<string>("--token");

        await this._sessionManager.LoadSettingsAsync();
        this._sessionManager.SetGitHubToken(ghToken);
        await this._sessionManager.SaveSettingsAsync();

        System.Console.WriteLine("Credentials have been set successfully.");
    }
}
