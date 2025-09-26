
using PromptusMaximus.Core.Models;

namespace GitHubModel.Core.Interfaces;

/// <summary>
/// Defines the contract for a client that interacts with GitHub Models API.
/// </summary>
public interface IModelsClient
{
    /// <summary>
    /// Asynchronously retrieves a collection of available GitHub models.
    /// </summary>
    /// <param name="ghToken">The GitHub authentication token used to authenticate API requests.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="GitHubModelCollection"/> with the available models.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ghToken"/> is null or empty.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
    Task<GitHubModelCollection> GetModelsAsync(string ghToken, CancellationToken cancellationToken = default);
}