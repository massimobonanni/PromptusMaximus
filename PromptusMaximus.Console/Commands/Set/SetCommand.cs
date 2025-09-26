using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Console.Commands.Set;
using PromptusMaximus.Core.Interfaces;

namespace PromptusMaximus.Console.Commands;

/// <summary>
/// Represents the main 'set' command that provides subcommands for configuring session settings.
/// This command serves as a container for various configuration options including credentials, model selection, and language preferences.
/// </summary>
internal class SetCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetCommand"/> class with the specified session manager.
    /// </summary>
    /// <param name="sessionManager">The session manager instance used to manage configuration settings across all subcommands.</param>
    public SetCommand(ISessionManager sessionManager) :
        base("set", "Configure session settings", sessionManager)
    {
        this.Subcommands.Add(new SetCredentialCommand(sessionManager));
        this.Subcommands.Add(new SetClearCommand(sessionManager));
        this.Subcommands.Add(new SetLanguageCommand(sessionManager));
        this.Subcommands.Add(new SetModelCommand(sessionManager));
        this.Subcommands.Add(new SetShowCommand(sessionManager));
    }
}
