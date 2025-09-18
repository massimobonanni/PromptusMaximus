using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

internal class SetLanguageCommand : CommandBase
{
    public SetLanguageCommand(ServiceProvider serviceProvider = null) :
        base("language", "Set default language", serviceProvider)
    {
        var languageOption = new Option<Core.Configuration.Languages>(name: "--language")
        {
            Description = "The default language (only en and it supported)",
            Required = true,
        };
        languageOption.Aliases.Add("-l");
        this.Options.Add(languageOption);

        this.SetAction(CommandHandler);
    }

    private async Task CommandHandler(ParseResult parseResult,CancellationToken cancellationToken)
    {
        var language= parseResult.GetValue<Core.Configuration.Languages>("--language");

        await this._sessionManager.LoadSettingsAsync();
        this._sessionManager.SetLanguage(language);
        await this._sessionManager.SaveSettingsAsync();

        System.Console.WriteLine("Default language have been set successfully.");
    }
}
