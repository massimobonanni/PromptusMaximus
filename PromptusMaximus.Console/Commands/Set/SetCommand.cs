using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Console.Commands.Set;

namespace PromptusMaximus.Console.Commands;

internal class SetCommand : CommandBase
{
    public SetCommand(ServiceProvider serviceProvider = null) :
        base("set", "Configure session settings", serviceProvider)
    {
        this.Subcommands.Add(new SetCredentialCommand(serviceProvider));
        this.Subcommands.Add(new SetClearCommand(serviceProvider));
    }

}
