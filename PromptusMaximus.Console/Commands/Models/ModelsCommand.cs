using GitHubModel.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using PromptusMaximus.Console.Commands.Models;
using PromptusMaximus.Core.Interfaces;
using System.CommandLine;

namespace PromptusMaximus.Console.Commands;

/// <summary>
/// Represents a command for managing GitHub Models operations within the PromptusMaximus console application.
/// This command serves as a container for model-related subcommands such as listing available models.
/// </summary>
internal class ModelsCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModelsCommand"/> class.
    /// </summary>
    /// <param name="sessionManager">The session manager for handling user session settings and configuration.</param>
    /// <param name="modelClient">The GitHub Models client for interacting with the GitHub Models API.</param>
    public ModelsCommand(ISessionManager sessionManager, IModelsClient modelClient) :
        base("models", "Allow to manage GitHub Models", sessionManager)
    {
        this.Subcommands.Add(new ModelsListCommand(sessionManager, modelClient));
    }
}
