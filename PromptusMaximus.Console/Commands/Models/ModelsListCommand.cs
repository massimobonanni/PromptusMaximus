using GitHubModel.Core.Interfaces;
using PromptusMaximus.Console.Utilities;
using PromptusMaximus.Core.Interfaces;
using PromptusMaximus.Core.Models;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Models;

internal class ModelsListCommand : CommandBase
{
    private IModelsClient _modelsClient;

    public ModelsListCommand(ISessionManager sessionManager, IModelsClient modelsClient) :
        base("list", "Retrieve the list of the Models available on GitHub", sessionManager)
    {
        this._modelsClient = modelsClient;
        this.SetAction(CommandHandler);
    }

    private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
    {
        await this._sessionManager.LoadSettingsAsync();

        var result = await this._modelsClient
                    .GetModelsAsync(this._sessionManager.GetGitHubToken(), cancellationToken)
                    .WithLoadingIndicator(
                        message: $"Retrieving Models from GitHub",
                        style: LoadingIndicator.Style.Spinner,
                        completionMessage: $"Models retrieved successfully!",
                        showTimeTaken: true);

        ConsoleUtility.WriteLine($"Total Models: {result.Count}", ConsoleColor.Yellow);
        System.Console.WriteLine();

        foreach (var model in result.Models)
        {
            model.Display();
            System.Console.WriteLine();
        }
    }
}
