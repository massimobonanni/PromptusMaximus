using System.Net.Http.Headers;
using System.Text.Json;
using GitHubModel.Core.Interfaces;
using PromptusMaximus.Core;
using PromptusMaximus.Core.Models;



namespace GitHubModel.Client.Services;

/// <summary>
/// Client for interacting with the GitHub Models Catalog API.
/// </summary>
public class GitHubModelsClient : IDisposable, IModelsClient
{
    private readonly HttpClient _httpClient;
    private readonly bool _disposeHttpClient;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes a new instance of the GitHubModelsCatalogClient class with a default HttpClient.
    /// </summary>
    public GitHubModelsClient() : this(new HttpClient())
    {
        _disposeHttpClient = true;
    }

    /// <summary>
    /// Initializes a new instance of the GitHubModelsCatalogClient class with a provided HttpClient.
    /// </summary>
    /// <param name="httpClient">The HttpClient to use for requests.</param>
    public GitHubModelsClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _disposeHttpClient = false;

        // Set default headers that don't change between requests
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        _httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
    }

    /// <summary>
    /// Gets the catalog of available GitHub models.
    /// </summary>
    /// <param name="ghToken">The GitHub token for authentication.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A collection of available GitHub models.</returns>
    /// <exception cref="ArgumentException">Thrown when the token is null or empty.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the token is invalid or insufficient permissions.</exception>
    /// <exception cref="HttpRequestException">Thrown when a network error occurs.</exception>
    /// <exception cref="InvalidOperationException">Thrown when an API or server error occurs.</exception>
    /// <exception cref="TimeoutException">Thrown when the request times out.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public async Task<GitHubModelCollection> GetModelsAsync(string ghToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ghToken))
            throw new ArgumentException("GitHub token cannot be null or empty.", nameof(ghToken));

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://models.github.ai/catalog/models");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ghToken);

            using var response = await _httpClient.SendAsync(request, cancellationToken);

            // Handle different HTTP status codes
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid GitHub token or insufficient permissions.");

                case System.Net.HttpStatusCode.Forbidden:
                    throw new UnauthorizedAccessException("Access forbidden. Check your GitHub token permissions.");

                case System.Net.HttpStatusCode.NotFound:
                    throw new InvalidOperationException("GitHub Models API endpoint not found.");

                case System.Net.HttpStatusCode.TooManyRequests:
                    throw new InvalidOperationException("Rate limit exceeded. Please try again later.");

                case var status when (int)status >= 400 && (int)status < 500:
                    var clientErrorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new InvalidOperationException($"Client error ({(int)status}): {clientErrorContent}");

                case var status when (int)status >= 500:
                    var serverErrorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new InvalidOperationException($"Server error ({(int)status}): {serverErrorContent}");
            }

            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(jsonContent))
                throw new InvalidOperationException("Received empty response from the API.");

            var models = JsonSerializer.Deserialize<PromptusMaximus.Core.Models.GitHubModel[]>(jsonContent, JsonOptions);

            if (models == null)
                throw new InvalidOperationException("Failed to deserialize the API response.");

            var collection = new GitHubModelCollection();
            collection.AddRange(models);

            return collection;
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
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse API response: {ex.Message}", ex);
        }
        catch (Exception ex) when (!(ex is ArgumentException || ex is UnauthorizedAccessException ||
                                   ex is InvalidOperationException || ex is TimeoutException ||
                                   ex is OperationCanceledException))
        {
            throw new InvalidOperationException($"An unexpected error occurred: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Disposes the HttpClient if it was created internally.
    /// </summary>
    public void Dispose()
    {
        if (_disposeHttpClient)
        {
            _httpClient?.Dispose();
        }
    }

}