# GitHubModels.Client

A .NET client library for interacting with the GitHub Models Catalog API. This library provides a simple and robust way to retrieve information about available AI models hosted on GitHub's platform.

## 🎯 Purpose

The GitHubModels.Client library serves as the core HTTP client component for the PromptusMaximus application, enabling seamless integration with GitHub's AI model catalog. It provides a clean abstraction layer for fetching model information, handling authentication, and managing API responses.

## 🏗️ Architecture

The library follows a clean architecture pattern with clear separation of concerns:

### Components

- **GitHubModelsClient**: The main HTTP client that handles API communication
- **IModelsClient**: Interface defining the contract for model catalog operations
- **Core Models Integration**: Leverages shared models from PromptusMaximus.Core

### Key Features

- ✅ **Robust Error Handling**: Comprehensive exception handling for various HTTP status codes
- ✅ **Authentication Support**: Bearer token authentication with GitHub tokens
- ✅ **Async/Await Pattern**: Full asynchronous support with cancellation tokens
- ✅ **JSON Serialization**: Snake_case property handling for GitHub API compatibility
- ✅ **Resource Management**: Proper disposal patterns for HTTP resources
- ✅ **Timeout Handling**: Built-in timeout and cancellation support

### Class Diagram

```mermaid
classDiagram
    class IModelsClient {
        <<interface>>
        +GetModelsAsync(ghToken: string, cancellationToken: CancellationToken) Task~GitHubModelCollection~
    }
    
    class GitHubModelsClient {
        -_httpClient: HttpClient
        -_disposeHttpClient: bool
        -JsonOptions: JsonSerializerOptions$
        +GitHubModelsClient()
        +GitHubModelsClient(httpClient: HttpClient)
        +GetModelsAsync(ghToken: string, cancellationToken: CancellationToken) Task~GitHubModelCollection~
        +Dispose() void
    }
    
    class GitHubModelCollection {
        -_models: List~GitHubModel~
        +Models: IReadOnlyList~GitHubModel~
        +Count: int
        +Add(model: GitHubModel) void
        +AddRange(models: IEnumerable~GitHubModel~) void
        +Clear() void
        +Contains(model: GitHubModel) bool
        +Remove(model: GitHubModel) bool
        +GetEnumerator() IEnumerator~GitHubModel~
    }
    
    class GitHubModel {
        +Id: string?
        +Name: string?
        +Registry: string?
        +Publisher: string?
        +Summary: string?
        +RateLimitTier: string?
        +HtmlUrl: string?
        +Version: string?
        +Capabilities: List~string~
        +Limits: GitHubModelLimits?
        +Tags: List~string~
        +SupportedInputModalities: List~string~
        +SupportedOutputModalities: List~string~
    }
    
    class GitHubModelLimits {
        +MaxInputTokens: int?
        +MaxOutputTokens: int?
    }
    
    class HttpClient {
        <<external>>
    }
    
    class IDisposable {
        <<interface>>
        +Dispose() void
    }
    
    class IEnumerable~T~ {
        <<interface>>
        +GetEnumerator() IEnumerator~T~
    }
    
    %% Relationships
    GitHubModelsClient ..|> IModelsClient : implements
    GitHubModelsClient ..|> IDisposable : implements
    GitHubModelsClient --> HttpClient : uses
    GitHubModelsClient --> GitHubModelCollection : returns
    GitHubModelCollection ..|> IEnumerable~T~ : implements
    GitHubModelCollection --> GitHubModel : contains
    GitHubModel --> GitHubModelLimits : has
    
    %% Styling
    classDef interfaceStyle fill:#e1f5fe,stroke:#01579b,stroke-width:2px
    classDef clientStyle fill:#f3e5f5,stroke:#4a148c,stroke-width:2px
    classDef modelStyle fill:#e8f5e8,stroke:#1b5e20,stroke-width:2px
    classDef externalStyle fill:#fff3e0,stroke:#e65100,stroke-width:2px
    
    class IModelsClient interfaceStyle
    class IDisposable interfaceStyle
    class IEnumerable~T~ interfaceStyle
    class GitHubModelsClient clientStyle
    class GitHubModelCollection modelStyle
    class GitHubModel modelStyle
    class GitHubModelLimits modelStyle
    class HttpClient externalStyle
```

## 🔧 Technical Details

### HTTP Client Configuration

The client is configured with:

- GitHub API version: `2022-11-28`
- Accept header: `application/vnd.github+json`
- Bearer token authentication
- Snake_case JSON property naming

### Error Handling

The client provides specific exception types for different scenarios:

| Status Code | Exception Type | Description |
|-------------|----------------|-------------|
| 401 | `UnauthorizedAccessException` | Invalid or missing GitHub token |
| 403 | `UnauthorizedAccessException` | Insufficient permissions |
| 404 | `InvalidOperationException` | API endpoint not found |
| 429 | `InvalidOperationException` | Rate limit exceeded |
| 4xx | `InvalidOperationException` | Client error with details |
| 5xx | `InvalidOperationException` | Server error with details |
| Timeout | `TimeoutException` | Request timeout |
| Cancelled | `OperationCanceledException` | Operation cancelled |

### API Endpoint

The client interacts with the GitHub Models Catalog API:

```text
https://models.github.ai/catalog/models
```

## 📦 Dependencies

### External Packages

- **System.Text.Json 9.0.0**: For JSON serialization and deserialization with modern performance optimizations

### Project References

- **PromptusMaximus.Core**: Provides shared models, interfaces, and core functionality including:
  - `GitHubModel` class
  - `GitHubModelCollection` class
  - `IModelsClient` interface

## 🚀 Usage Example

```csharp
using GitHubModel.Client.Services;

// Create client instance
using var client = new GitHubModelsClient();

// Fetch available models
var models = await client.GetModelsAsync("your-github-token");

// Access model information
foreach (var model in models)
{
    Console.WriteLine($"Model: {model.Name} by {model.Publisher}");
}
```

## 🔒 Authentication

The client requires a GitHub token with appropriate permissions to access the Models API. The token should be passed to the `GetModelsAsync` method for each request.

## 🎯 Target Framework

- **.NET 9.0**: Built on the latest .NET platform with modern C# features
- **Nullable Reference Types**: Enabled for better null safety
- **Implicit Usings**: Simplified using statements for common namespaces

## 📄 License

This project is part of the PromptusMaximus application suite. See the main project LICENSE file for details.

## 🤝 Contributing

This library is part of the larger PromptusMaximus project. Please refer to the main project repository for contribution guidelines and development setup instructions.