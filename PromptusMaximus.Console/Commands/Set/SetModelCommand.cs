using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

/// <summary>
/// Command for setting the default AI model used in the session.
/// Provides functionality to configure which GitHub Model will be used for AI operations.
/// </summary>
internal class SetModelCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetModelCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager instance for handling configuration persistence.</param>
    public SetModelCommand(ISessionManager sessionManager) :
        base("model", "Set default model", sessionManager)
    {
        var modelOption = new Option<string>(name: "--model")
        {
            Description = "The default GitHub Model (see https://github.com/marketplace?type=models)",
            Required = true,
        };
        modelOption.Aliases.Add("-m");
        this.Options.Add(modelOption);

        this.SetAction(CommandHandler);
    }

    /// <summary>
    /// Handles the execution of the set model command.
    /// Loads current settings, updates the model configuration, and saves the changes.
    /// </summary>
    /// <param name="parseResult">The parsed command line arguments containing the model value.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous command execution operation.</returns>
    private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var model = parseResult.GetValue<string>("--model");

        await this._sessionManager.LoadSettingsAsync();
        this._sessionManager.SetModel(model);
        await this._sessionManager.SaveSettingsAsync();

        System.Console.WriteLine("Default model have been set successfully.");
    }
}
