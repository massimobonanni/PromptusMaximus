using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Console.Services;
using PromptusMaximus.Console.Utilities;
using PromptusMaximus.Core.Interfaces;

namespace PromptusMaximus.Console.Commands;

internal class TranslateCommand : CommandBase
{
    private readonly IModelsService _modelsService;

    public TranslateCommand(ISessionManager sessionManager, IModelsService modelsService) :
        base("translate", "Translate a sentence as a Roman", sessionManager)
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
