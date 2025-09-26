using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Console.Commands.Set;
using PromptusMaximus.Core.Interfaces;

namespace PromptusMaximus.Console.Commands;

internal class SetCommand : CommandBase
{
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
