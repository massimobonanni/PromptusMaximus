using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

internal class SetClearCommand : CommandBase
{
    public SetClearCommand(ServiceProvider serviceProvider = null) :
        base("clear", "Clear settings (also credentials)", serviceProvider)
    {
        this.SetAction(CommandHandler);
    }

    private async Task CommandHandler(ParseResult parseResult,CancellationToken cancellationToken)
    {
        System.Console.WriteLine("Are you sure you want to clear all settings? This action cannot be undone. (yes/no)");
        var confirmation=System.Console.ReadLine();
        if(confirmation?.ToLower()!="yes")
        {
            System.Console.WriteLine("Operation cancelled.");
            return;
        }
        
        await this._sessionManager.ClearAllSettingsAsync();

        System.Console.WriteLine("All settings have been cleared successfully.");
    }
}
