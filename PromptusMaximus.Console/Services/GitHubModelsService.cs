using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromptusMaximus.Core.Configuration;
using Azure;
using Azure.AI.Inference;
using PromptusMaximus.Console.Utilities;
using System.Net.Http;
using System.Net;
using PromptusMaximus.Core.Interfaces;

namespace PromptusMaximus.Console.Services;

/// <summary>
/// Provides AI model completion services using GitHub Models API.
/// Implements the <see cref="IModelsService"/> interface to interact with GitHub's AI inference endpoint.
/// </summary>
internal class GitHubModelsService : IModelsService
{
    /// <summary>
    /// The GitHub Models API inference endpoint URI.
    /// </summary>
    private Uri endpoint = new Uri("https://models.github.ai/inference");

    /// <summary>
    /// Completes a prompt using the specified AI model through GitHub Models API.
    /// </summary>
    /// <param name="modelName">The name of the AI model to use for completion. Cannot be null or empty.</param>
    /// <param name="prompt">The input prompt to be completed by the model. Cannot be null or empty.</param>
    /// <param name="ghToken">The GitHub token for authentication and API access. Cannot be null or empty.</param>
    /// <param name="language">The language setting for the completion request, used to determine the system prompt.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests during the async operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the completed text as a string.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="modelName"/>, <paramref name="prompt"/>, or <paramref name="ghToken"/> is null or empty,
    /// or when the specified model is not found or not available, or when a client error occurs.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when the GitHub token is invalid or has insufficient permissions.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the API returns a null or empty response, when rate limits are exceeded,
    /// when a server error occurs, when a network error occurs, or when any unexpected error occurs.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Thrown when the request times out.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is cancelled via the <paramref name="cancellationToken"/>.
    /// </exception>
    public async Task<string> CompleteAsync(string modelName, string prompt, string ghToken,
        Languages language, CancellationToken cancellationToken)
    {
        // Input validation
        if (string.IsNullOrWhiteSpace(modelName))
            throw new ArgumentException("Model name cannot be null or empty.", nameof(modelName));

        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("Prompt cannot be null or empty.", nameof(prompt));

        if (string.IsNullOrWhiteSpace(ghToken))
            throw new ArgumentException("GitHub token cannot be null or empty.", nameof(ghToken));

        var credential = new Azure.AzureKeyCredential(ghToken);

        var client = new ChatCompletionsClient(
            endpoint,
            credential,
            new AzureAIInferenceClientOptions()
        );

        var systemPrompt = await PromptFileUtility.GetSystemPromptAsync(language);

        var requestOptions = new ChatCompletionsOptions()
        {
            Messages =
                {
                    new ChatRequestSystemMessage(systemPrompt),
                    new ChatRequestUserMessage(prompt),
                },
            Model = modelName,
        };

        try
        {
            Response<ChatCompletions> response = await client.CompleteAsync(requestOptions, cancellationToken);

            if (response?.Value?.Content == null)
                throw new InvalidOperationException("Received null or empty response from the API.");

            return response.Value.Content;
        }
        catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("Invalid GitHub token or insufficient permissions.", ex);
        }
        catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
        {
            throw new ArgumentException($"Model '{modelName}' not found or not available.", nameof(modelName), ex);
        }
        catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.TooManyRequests)
        {
            throw new InvalidOperationException("Rate limit exceeded. Please try again later.", ex);
        }
        catch (RequestFailedException ex) when (ex.Status >= 400 && ex.Status < 500)
        {
            throw new ArgumentException($"Client error: {ex.Message}", ex);
        }
        catch (RequestFailedException ex) when (ex.Status >= 500)
        {
            throw new InvalidOperationException($"Server error: {ex.Message}", ex);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException($"API request failed: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            throw new TimeoutException("The request timed out.", ex);
        }
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("The operation was cancelled.", cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Network error occurred: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An unexpected error occurred: {ex.Message}", ex);
        }
    }
}
