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

internal class GitHubModelsService : IModelsService
{
    private Uri endpoint = new Uri("https://models.github.ai/inference");

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
