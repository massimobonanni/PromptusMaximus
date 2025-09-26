using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

internal class SetModelCommand : CommandBase
{
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

    private async Task CommandHandler(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var model = parseResult.GetValue<string>("--model");

        await this._sessionManager.LoadSettingsAsync();
        this._sessionManager.SetModel(model);
        await this._sessionManager.SaveSettingsAsync();

        System.Console.WriteLine("Default model have been set successfully.");
    }
}
