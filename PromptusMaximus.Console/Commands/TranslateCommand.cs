using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Console.Services;
using PromptusMaximus.Console.Utilities;
using PromptusMaximus.Core.Interfaces;

namespace PromptusMaximus.Console.Commands;

/// <summary>
/// Command for translating text using AI models with Roman-style translation capabilities.
/// Supports multiple models and provides formatted output with loading indicators.
/// </summary>
internal class TranslateCommand : CommandBase
{
    /// <summary>
    /// Service for interacting with AI models to perform text completions and translations.
    /// </summary>
    private readonly IModelsService _modelsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager for handling user settings and authentication.</param>
    /// <param name="modelsService">The service for interacting with AI models.</param>
    public TranslateCommand(ISessionManager sessionManager, IModelsService modelsService) :
        base("translate", "Translate a sentence as an old Roman", sessionManager)
    {
        this._modelsService = modelsService;

        var textOption = new Option<string>(name: "--text")
        {
            Description = "The text to translate",
            Required = true,
        };
        textOption.Aliases.Add("-t");
        this.Options.Add(textOption);

        var modelsOption = new Option<List<string>>(name: "--model")
        {
            AllowMultipleArgumentsPerToken = true,
            Description = "Models to use. If you don;t set this option, the default model is used",
            Required = false,
        };
        modelsOption.Aliases.Add("-m");
        this.Options.Add(modelsOption);

        this.SetAction(CommandHandler);
    }

    /// <summary>
    /// Handles the execution of the translate command by processing the input text through specified AI models.
    /// </summary>
    /// <param name="parseResult">The parsed command line arguments containing text and model options.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests during the async operation.</param>
    /// <returns>A task that represents the asynchronous command execution.</returns>
    /// <remarks>
    /// This method performs the following operations:
    /// <list type="number">
    /// <item><description>Loads current session settings</description></item>
    /// <item><description>Extracts text and model parameters from command line arguments</description></item>
    /// <item><description>Uses default model if no models are specified</description></item>
    /// <item><description>Processes translation through each specified model with loading indicators</description></item>
    /// <item><description>Displays results with color-coded output for success and error cases</description></item>
    /// </list>
    /// </remarks>
    private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
    {
        await this._sessionManager.LoadSettingsAsync();

        var text = parseResult.GetValue<string>("--text");

        ConsoleUtility.WriteLine($"Translating the following text:\n\t\"{text}\"\n", ConsoleColor.Magenta);

        var models = parseResult.GetValue<List<string>>("--model");
        if (models == null || !models.Any())
        {
            models = new List<string>() { _sessionManager.CurrentSettings.Model };
        }

        if (!models.Any())
        {
            ConsoleUtility.WriteLine("No models specified and no default model set in settings.", ConsoleColor.Red);
            return;
        }

        foreach (var model in models)
        {
            try
            {
                // Display the model being used
                ConsoleUtility.WriteLine($"Model: {model}", ConsoleColor.Green);

                // Use the loading indicator with the API call
                var result = await this._modelsService
                    .CompleteAsync(model, text, this._sessionManager.GetGitHubToken(),
                        this._sessionManager.CurrentSettings.Language, cancellationToken)
                    .WithLoadingIndicator(
                        message: $"Translating with {model}",
                        style: LoadingIndicator.Style.Spinner,
                        completionMessage: $"\tTranslated with {model}",
                        showTimeTaken: true,
                        completionColor: ConsoleColor.Yellow);

                ConsoleUtility.WriteLine(result);
                ConsoleUtility.WriteLine(); // Add spacing between models
            }
            catch (Exception ex)
            {
                ConsoleUtility.WriteLine($"Error with model {model}: {ex.Message}", ConsoleColor.Red);
            }
        }
    }
}
