using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

internal class SetShowCommand : CommandBase
{
    public SetShowCommand(ServiceProvider serviceProvider = null) :
        base("show", "Show current settings", serviceProvider)
    {
        this.SetAction(CommandHandler);
    }

    private async Task CommandHandler(ParseResult parseResult,CancellationToken cancellationToken)
    {
        await this._sessionManager.LoadSettingsAsync();
        System.Console.WriteLine($"Default model : {this._sessionManager.CurrentSettings.Model}");
        System.Console.WriteLine($"Default language : {this._sessionManager.CurrentSettings.Language}");
    }
}
