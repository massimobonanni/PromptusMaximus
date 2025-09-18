using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using GitHubModel.Client.Services;
using PromptusMaximus.Console.Utilities;
using GitHubModel.Client.Models;

namespace PromptusMaximus.Console.Commands.Models;

internal class ModelsListCommand : CommandBase
{
    private GitHubModelsClient _gitHubModelsClient;

    public ModelsListCommand(ServiceProvider serviceProvider) :
        base("list", "Retrieve the list of the Models available on GitHub", serviceProvider)
    {
        this._gitHubModelsClient = serviceProvider.GetRequiredService<GitHubModelsClient>();
        this.SetAction(CommandHandler);
    }

    private async Task CommandHandler(ParseResult parseResult,CancellationToken cancellationToken)
    {
        await this._sessionManager.LoadSettingsAsync();

        var result = await this._gitHubModelsClient
                    .GetModelsAsync(this._sessionManager.GetGitHubToken(), cancellationToken)
                    .WithLoadingIndicator(
                        message: $"Retrieving Models from GitHub",
                        style: LoadingIndicator.Style.Spinner,
                        completionMessage: $"Models retrieved successfully!",
                        showTimeTaken:true);

        ConsoleUtility.WriteLine($"Total Models: {result.Count}", ConsoleColor.Yellow);
        System.Console.WriteLine();

        foreach (var model in result.Models)
        {
            model.Display();
            System.Console.WriteLine();
        }
    }
}
