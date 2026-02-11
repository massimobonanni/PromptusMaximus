using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Console.Services;
using PromptusMaximus.Console.Utilities;
using PromptusMaximus.Core.Configuration;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;
using System.Reflection;

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

    private readonly Option<string> _textOption;

    private readonly Option<bool> _mascotOption;

    private readonly Option<string[]> _modelsOption;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager for handling user settings and authentication.</param>
    /// <param name="modelsService">The service for interacting with AI models.</param>
    public TranslateCommand(ISessionManager sessionManager, IModelsService modelsService) :
        base("translate", "Translate a sentence as an old Roman", sessionManager)
    {
        this._modelsService = modelsService;

        _mascotOption = new Option<bool>("--mascotMode")
        {
            Description = "Enable the mascotte mode",
            Required = false,
            DefaultValueFactory = (r) => false
        };
        _mascotOption.Aliases.Add("-mm");
        this.Options.Add(_mascotOption);

        _textOption = new Option<string>(name: "--text")
        {
            Description = "The text to translate",
            Required = true,
        };
        _textOption.Aliases.Add("-t");
        this.Options.Add(_textOption);

        _textOption.Validators.Add(result =>
        {
            var value = result.GetValue(_textOption);
            if (string.IsNullOrWhiteSpace(value))
            {
                result.AddError("The --text option cannot be empty.");
            }
        });

        _modelsOption = new Option<string[]>(name: "--model")
        {
            AllowMultipleArgumentsPerToken = true,
            Arity = ArgumentArity.ZeroOrMore,
            Description = "Models to use. If you don't set this option, the default model is used",
            Required = false,
        };
        _modelsOption.Aliases.Add("-m");

        _modelsOption.Validators.Add(result =>
        {
            var value = result.GetValue(_modelsOption);
            if ((value == null || !value.Any() || value.All(string.IsNullOrWhiteSpace)) &&
                string.IsNullOrWhiteSpace(sessionManager.CurrentSettings.Model))
            {
                result.AddError("At least one model must be specified using --model option or a default model must be set in settings.");
            }
        });

        _modelsOption.CustomParser = result =>
        {
            string[] values = null;
            if (result.Tokens.Any())
            {
                values = result.Tokens.Select(t => t.Value).Where(v => !string.IsNullOrWhiteSpace(v)).ToArray();
            }
            return values;
        };
        this.Options.Add(_modelsOption);

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

        var text = parseResult.GetValue(_textOption);
        var mascotMode = parseResult.GetValue(_mascotOption);

        ConsoleUtility.WriteLine($"Translating the following text:\n\t\"{text}\"\n", ConsoleColor.Magenta);

        var models = parseResult.GetValue(_modelsOption);
        if ((models == null || !models.Any()) && !string.IsNullOrEmpty(_sessionManager.CurrentSettings.Model))
        {
            models = new string[] { _sessionManager.CurrentSettings.Model };
        }

        foreach (var model in models)
        {
            try
            {
                // Display the model being used
                ConsoleUtility.WriteLine($"Model: {model}", ConsoleColor.Green);

                // Use the loading indicator with the API call
                var result = await this._modelsService
                    .CompleteAsync(model, text, mascotMode, this._sessionManager.GetGitHubToken(),
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
                parseResult.CommandResult.AddError($"Error with model {model}: {ex.Message}");
            }
        }
    }
}
