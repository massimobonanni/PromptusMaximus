using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

internal class SetCredentialCommand : CommandBase
{
    public SetCredentialCommand(ServiceProvider serviceProvider = null) :
        base("credential", "Set authentication credentials", serviceProvider)
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

    private async Task CommandHandler(ParseResult parseResult,CancellationToken cancellationToken)
    {
        var ghToken= parseResult.GetValue<string>("--token");

        await this._sessionManager.LoadSettingsAsync();
        this._sessionManager.SetGitHubToken(ghToken);
        await this._sessionManager.SaveSettingsAsync();

        System.Console.WriteLine("Credentials have been set successfully.");
    }
}
