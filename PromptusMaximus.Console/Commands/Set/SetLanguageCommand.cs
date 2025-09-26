using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands.Set;

/// <summary>
/// Command that allows users to set the default language preference for their session.
/// Supports setting the language through command-line options with validation.
/// </summary>
internal class SetLanguageCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetLanguageCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager instance used to persist language settings.</param>
    public SetLanguageCommand(ISessionManager sessionManager) :
        base("language", "Set default language", sessionManager)
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

    /// <summary>
    /// Handles the execution of the set language command by retrieving the language option,
    /// updating the session settings, and persisting the changes.
    /// </summary>
    /// <param name="parseResult">The parsed command-line arguments containing the language option value.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous command execution operation.</returns>
    private async Task CommandHandler(ParseResult parseResult,CancellationToken cancellationToken)
    {
        var language= parseResult.GetValue<Core.Configuration.Languages>("--language");

        await this._sessionManager.LoadSettingsAsync();
        this._sessionManager.SetLanguage(language);
        await this._sessionManager.SaveSettingsAsync();

        System.Console.WriteLine("Default language have been set successfully.");
    }
}
