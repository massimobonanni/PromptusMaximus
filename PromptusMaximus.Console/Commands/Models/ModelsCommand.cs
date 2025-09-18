using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using PromptusMaximus.Console.Commands.Models;

namespace PromptusMaximus.Console.Commands;

internal class ModelsCommand : CommandBase
{
    public ModelsCommand(ServiceProvider serviceProvider) :
        base("models", "Allow to manage GitHub Models", serviceProvider)
    {
        this.Subcommands.Add(new ModelsListCommand(serviceProvider));
    }

}
