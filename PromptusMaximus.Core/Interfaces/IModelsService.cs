using PromptusMaximus.Core.Configuration;

namespace PromptusMaximus.Core.Interfaces;

/// <summary>
/// Provides services for interacting with AI models to generate text completions.
/// </summary>
public interface IModelsService
{
    /// <summary>
    /// Completes a prompt using the specified AI model asynchronously.
    /// </summary>
    /// <param name="modelName">The name of the AI model to use for completion.</param>
    /// <param name="prompt">The input prompt to be completed by the model.</param>
    /// <param name="mascotMode">Indicates whether to enable mascotte mode for the completion.</param>
    /// <param name="ghToken">The GitHub token for authentication and API access.</param>
    /// <param name="language">The language setting for the completion request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests during the async operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the completed text as a string.</returns>
    Task<string> CompleteAsync(string modelName, string prompt, bool mascotMode, string ghToken, Languages language, CancellationToken cancellationToken);
}