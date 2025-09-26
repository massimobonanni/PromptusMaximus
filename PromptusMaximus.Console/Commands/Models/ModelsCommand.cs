using GitHubModel.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Console.Commands.Models;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands;

internal class ModelsCommand : CommandBase
{
    public ModelsCommand(ISessionManager sessionManager, IModelsClient modelClient) :
        base("models", "Allow to manage GitHub Models", sessionManager)
    {
        this.Subcommands.Add(new ModelsListCommand(sessionManager, modelClient));
    }

}
