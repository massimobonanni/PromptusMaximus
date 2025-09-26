using GitHubModel.Core.Interfaces;
using PromptusMaximus.Console.Utilities;
using PromptusMaximus.Core.Interfaces;
using PromptusMaximus.Core.Models;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Models;

/// <summary>
/// Command for retrieving and displaying the list of available GitHub models.
/// This command provides functionality to list all models accessible through the GitHub Models API.
/// </summary>
internal class ModelsListCommand : CommandBase
{
    /// <summary>
    /// The client used to interact with the GitHub Models API.
    /// </summary>
    private IModelsClient _modelsClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelsListCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager for handling user settings and authentication.</param>
    /// <param name="modelsClient">The client for interacting with the GitHub Models API.</param>
    public ModelsListCommand(ISessionManager sessionManager, IModelsClient modelsClient) :
        base("list", "Retrieve the list of the Models available on GitHub", sessionManager)
    {
        this._modelsClient = modelsClient;
        this.SetAction(CommandHandler);
    }

    /// <summary>
    /// Handles the execution of the models list command.
    /// Retrieves the available GitHub models and displays them in the console with a loading indicator.
    /// </summary>
    /// <param name="parseResult">The parsed command line arguments.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous command execution.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the GitHub token is null or empty.</exception>
    /// <exception cref="HttpRequestException">Thrown when the API request fails.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
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
