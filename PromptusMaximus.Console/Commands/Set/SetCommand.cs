using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Console.Commands.Set;

namespace PromptusMaximus.Console.Commands;

internal class SetCommand : CommandBase
{
    public SetCommand(ServiceProvider serviceProvider) :
        base("set", "Configure session settings", serviceProvider)
    {
        this.Subcommands.Add(new SetCredentialCommand(serviceProvider));
        this.Subcommands.Add(new SetClearCommand(serviceProvider));
        this.Subcommands.Add(new SetLanguageCommand(serviceProvider));
        this.Subcommands.Add(new SetModelCommand(serviceProvider));
        this.Subcommands.Add(new SetShowCommand(serviceProvider));
    }

}
