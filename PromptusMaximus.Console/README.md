# 🏛️ Promptus Maximus Console

**The command-line interface for speaking like Caesar and debugging like a gladiator.**

The Promptus Maximus Console is a .NET command-line application that provides an interface for translating modern phrases into theatrical, Latin-inspired expressions using GitHub-hosted LLMs.

---

## 📋 Commands

The Promptus Maximus Console supports the following commands:

### `set` - Configure Session Settings

Manage application configuration and credentials.

#### `set credential` - Set Authentication Credentials

Configure your GitHub token for accessing GitHub Models.

**Usage:**
```bash
PromptusMaximus set credential --token YOUR_GITHUB_TOKEN
```

**Options:**
- `-t, --token` (required): GitHub Personal Access Token with GitHub Models access

**Example:**
```bash
PromptusMaximus set credential --token ghp_your_token_here
```

#### `set language` - Set Default Language

Configure the default language for translations.

**Usage:**
```bash
PromptusMaximus set language --language LANGUAGE
```

**Options:**
- `-l, --language` (required): The default language (currently supports `en` and `it`)

**Examples:**
```bash
PromptusMaximus set language --language en
PromptusMaximus set language -l it
```

#### `set model` - Set Default Model

Configure the default GitHub Model to use for translations.

**Usage:**
```bash
PromptusMaximus set model --model MODEL_NAME
```

**Options:**
- `-m, --model` (required): The default GitHub Model name (see [GitHub Models Marketplace](https://github.com/marketplace?type=models))

**Examples:**
```bash
PromptusMaximus set model --model gpt-4o
PromptusMaximus set model -m claude-3-sonnet
```

#### `set show` - Show Current Settings

Display the current application settings.

**Usage:**
```bash
PromptusMaximus set show
```

**Example Output:**
```
Default model : gpt-4o
Default language : en
GitHub Token : ghp_**********************
```

#### `set clear` - Clear All Settings

Remove all stored settings and credentials. This action requires confirmation.

**Usage:**
```bash
PromptusMaximus set clear
```

**Example:**
```bash
PromptusMaximus set clear
Are you sure you want to clear all settings? This action cannot be undone. (yes/no)
yes
All settings have been cleared successfully.
```

### `translate` - Translate Text to Roman Style

Translates a sentence as a Roman would speak it.

**Usage:**
```bash
PromptusMaximus translate --text "Your text here" [--model model1 model2]
```

**Options:**
- `-t, --text` (required): The text to translate
- `-m, --model` (optional): Models to use for translation. If not specified, uses the default model

**Examples:**
```bash
# Basic translation using default model
PromptusMaximus translate --text "I need coffee"

# Translation using specific models
PromptusMaximus translate --text "My code won't compile" --model gpt-4o gpt-3.5-turbo

# Using short form aliases
PromptusMaximus translate -t "I'm stuck in traffic" -m gpt-4o
```

### `models` - Manage GitHub Models

Work with available GitHub Models.

#### `models list` - List Available Models

Retrieve and display all available GitHub Models.

**Usage:**
```bash
PromptusMaximus models list
```

**Example Output:**
```
Retrieving Models from GitHub... ✓
Models retrieved successfully! (1.2s)

Total Models: 15

Model: gpt-4o
Publisher: OpenAI
Description: GPT-4o is OpenAI's most advanced multimodal model...
Task: chat-completion
```

---

## 🔧 Configuration

### First Time Setup

1. **Set your GitHub token:**
   ```bash
   PromptusMaximus set credential --token YOUR_GITHUB_TOKEN
   ```

2. **Set your preferred language:**
   ```bash
   PromptusMaximus set language --language en
   ```

3. **Set your default model:**
   ```bash
   PromptusMaximus set model --model gpt-4o
   ```

4. **Verify your settings:**
   ```bash
   PromptusMaximus set show
   ```

### GitHub Token Requirements

Your GitHub Personal Access Token needs access to GitHub Models. You can:
1. Go to [GitHub Settings > Developer settings > Personal access tokens](https://github.com/settings/tokens)
2. Create a new token with appropriate permissions
3. Use the token with the `set credential` command

---

## 📝 Examples

### Quick Translation
```bash
# Set up (first time only)
PromptusMaximus set credential --token ghp_your_token_here
PromptusMaximus set model --model gpt-4o
PromptusMaximus set language --language en

# Translate a phrase
PromptusMaximus translate --text "I'm debugging this code"
```

### Compare Multiple Models
```bash
PromptusMaximus translate --text "My Wi-Fi is down" --model gpt-4o claude-3-sonnet
```

### Check Available Models
```bash
PromptusMaximus models list
```

---

## 🏗️ Architecture

The console application is built with:

- **System.CommandLine**: Modern command-line interface framework
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **GitHub Models Client**: Custom client for GitHub Models API
- **Session Management**: Secure credential storage and configuration persistence
- **Loading Indicators**: Visual feedback during API calls

### Class Diagram

```mermaid
classDiagram
    %% Command Layer - Console Application
    class Program {
        <<static>>
        +Main(args string[]) Task~int~
    }
    
    class CommandBase {
        <<abstract>>
        #_sessionManager ISessionManager
        +CommandBase(name string, description string, sessionManager ISessionManager)
    }
    
    class SetCommand {
        +SetCommand(sessionManager ISessionManager)
    }
    
    class TranslateCommand {
        -_modelsService IModelsService
        +TranslateCommand(sessionManager ISessionManager, modelsService IModelsService)
        -CommandHandler(parseResult ParseResult, cancellationToken CancellationToken) Task
    }
    
    class ModelsCommand {
        +ModelsCommand(sessionManager ISessionManager, modelClient IModelsClient)
    }
    
    %% Set Command Subcommands
    class SetCredentialCommand {
        +SetCredentialCommand(sessionManager ISessionManager)
        -CommandHandler(parseResult ParseResult, cancellationToken CancellationToken) Task
    }
    
    class SetLanguageCommand {
        +SetLanguageCommand(sessionManager ISessionManager)
        -CommandHandler(parseResult ParseResult, cancellationToken CancellationToken) Task
    }
    
    class SetModelCommand {
        +SetModelCommand(sessionManager ISessionManager)
        -CommandHandler(parseResult ParseResult, cancellationToken CancellationToken) Task
    }
    
    class SetShowCommand {
        +SetShowCommand(sessionManager ISessionManager)
        -CommandHandler(parseResult ParseResult, cancellationToken CancellationToken) Task
    }
    
    class SetClearCommand {
        +SetClearCommand(sessionManager ISessionManager)
        -CommandHandler(parseResult ParseResult, cancellationToken CancellationToken) Task
    }
    
    class ModelsListCommand {
        -_modelsClient IModelsClient
        +ModelsListCommand(sessionManager ISessionManager, modelsClient IModelsClient)
        -CommandHandler(parseResult ParseResult, cancellationToken CancellationToken) Task
    }
    
    %% Service Layer - Console
    class GitHubModelsService {
        -endpoint Uri
        +CompleteAsync(modelName string, prompt string, ghToken string, language Languages, cancellationToken CancellationToken) Task~string~
    }
    
    %% Utility Layer - Console
    class ConsoleUtility {
        <<static>>
        +WriteLine(message string, foregroundColor ConsoleColor) void
        +Write(message string, foregroundColor ConsoleColor) void
        +WriteApplicationBanner() void
    }
    
    class LoadingIndicator {
        -_characters char[]
        -_message string
        -_cancellationTokenSource CancellationTokenSource
        +LoadingIndicator(message string, style Style, intervalMs int)
        +Stop() void
        +Complete(completionMessage string, showTimeTaken bool, color ConsoleColor) void
        +Dispose() void
    }
    
    class PromptFileUtility {
        <<static>>
        +GetSystemPromptAsync(language Languages) Task~string~
        +GetSystemPrompt(language Languages) string?
    }
    
    %% Core Interface Layer
    class ISessionManager {
        <<interface>>
        +CurrentSettings SessionSettings
        +LoadSettingsAsync() Task
        +SaveSettingsAsync() Task
        +ClearAllSettingsAsync(deleteFiles bool) Task
        +SetModel(model string) void
        +SetLanguage(language Languages) void
        +SetGitHubToken(token string) void
        +GetGitHubToken() string?
        +SetSecret(key string, value string) void
        +GetSecret(key string) string?
        +SetSetting(key string, value string) void
        +GetSetting(key string) string?
    }
    
    class IModelsService {
        <<interface>>
        +CompleteAsync(modelName string, prompt string, ghToken string, language Languages, cancellationToken CancellationToken) Task~string~
    }
    
    class IModelsClient {
        <<interface>>
        +GetModelsAsync(ghToken string, cancellationToken CancellationToken) Task~GitHubModelCollection~
    }
    
    class IProtectedDataProvider {
        <<interface>>
        +ProtectAsync(data byte[], optionalEntropy string?) Task~byte[]~
        +UnprotectAsync(encryptedData byte[], optionalEntropy string?) Task~byte[]~
    }
    
    %% Core Configuration Layer
    class SessionManager {
        -_protectedDataProvider IProtectedDataProvider
        -_currentSettings SessionSettings
        +CurrentSettings SessionSettings
        +LoadSettingsAsync() Task
        +SaveSettingsAsync() Task
        +ClearAllSettingsAsync(deleteFiles bool) Task
        +SetModel(model string) void
        +SetLanguage(language Languages) void
        +SetGitHubToken(token string) void
        +GetGitHubToken() string?
        +SetSecret(key string, value string) void
        +GetSecret(key string) string?
        +SetSetting(key string, value string) void
        +GetSetting(key string) string?
    }
    
    class SessionSettings {
        +Model string?
        +Language Languages
        +CustomSettings Dictionary~string, string~
        +Secrets Dictionary~string, string~
    }
    
    class Languages {
        <<enumeration>>
        en
        it
    }
    
    %% Core Security Layer
    class ProtectedDataProviderFactory {
        <<static>>
        +Create() IProtectedDataProvider
    }
    
    class WindowsProtectedDataProvider {
        +ProtectAsync(data byte[], optionalEntropy string?) Task~byte[]~
        +UnprotectAsync(encryptedData byte[], optionalEntropy string?) Task~byte[]~
    }
    
    class LinuxProtectedDataProvider {
        -KeySize int$
        -IvSize int$
        +ProtectAsync(data byte[], optionalEntropy string?) Task~byte[]~
        +UnprotectAsync(encryptedData byte[], optionalEntropy string?) Task~byte[]~
        -GenerateKeyAsync(optionalEntropy string?) Task~byte[]~
    }
    
    %% Core Models Layer
    class GitHubModel {
        +Id string?
        +Name string?
        +Registry string?
        +Publisher string?
        +Summary string?
        +RateLimitTier string?
        +HtmlUrl string?
        +Version string?
        +Capabilities List~string~
        +Limits GitHubModelLimits?
        +Tags List~string~
        +SupportedInputModalities List~string~
        +SupportedOutputModalities List~string~
    }
    
    class GitHubModelLimits {
        +MaxInputTokens int?
        +MaxOutputTokens int?
    }
    
    class GitHubModelCollection {
        -_models List~GitHubModel~
        +Models IReadOnlyList~GitHubModel~
        +Count int
        +Add(model GitHubModel) void
        +AddRange(models IEnumerable~GitHubModel~) void
        +Remove(model GitHubModel) bool
        +Clear() void
        +GetByPublisher(publisher string) IEnumerable~GitHubModel~
        +GetByCapability(capability string) IEnumerable~GitHubModel~
        +GetByTag(tag string) IEnumerable~GitHubModel~
    }
    
    %% Client Layer
    class GitHubModelsClient {
        -_httpClient HttpClient
        -_disposeHttpClient bool
        -JsonOptions JsonSerializerOptions$
        +GitHubModelsClient()
        +GitHubModelsClient(httpClient HttpClient)
        +GetModelsAsync(ghToken string, cancellationToken CancellationToken) Task~GitHubModelCollection~
        +Dispose() void
    }
    
    %% Extension Classes
    class ServiceProviderExtensions {
        <<static>>
        +GetModelsClient(provider ServiceProvider) IModelsClient
        +GetModelsService(provider ServiceProvider) IModelsService
        +GetSessionManager(provider ServiceProvider) ISessionManager
    }
    
    class GitHubModelExtensions {
        <<static>>
        +Display(model GitHubModel) void
    }
    
    class LoadingIndicatorExtensions {
        <<static>>
        +WithLoadingIndicator~T~(task Task~T~, message string, style Style, completionMessage string?, completionColor ConsoleColor, showTimeTaken bool, intervalMs int) Task~T~
        +WithLoadingIndicator(task Task, message string, style Style, completionMessage string?, completionColor ConsoleColor, showTimeTaken bool, intervalMs int) Task
    }
    
    class StringExtensions {
        <<static>>
        +Mask(input string, maskCharacter char, visibleCharacters int) string
        +MaskStringBothEnds(input string, visibleStartCharacters int, visibleEndCharacters int, maskCharacter char) string
    }
    
    %% Relationships
    Program --> SetCommand : creates
    Program --> TranslateCommand : creates
    Program --> ModelsCommand : creates
    Program --> ServiceProviderExtensions : uses
    
    CommandBase <|-- SetCommand
    CommandBase <|-- TranslateCommand
    CommandBase <|-- ModelsCommand
    
    SetCommand --> SetCredentialCommand : contains
    SetCommand --> SetLanguageCommand : contains
    SetCommand --> SetModelCommand : contains
    SetCommand --> SetShowCommand : contains
    SetCommand --> SetClearCommand : contains
    
    ModelsCommand --> ModelsListCommand : contains
    
    CommandBase --> ISessionManager : uses
    TranslateCommand --> IModelsService : uses
    ModelsListCommand --> IModelsClient : uses
    
    GitHubModelsService ..|> IModelsService : implements
    GitHubModelsClient ..|> IModelsClient : implements
    SessionManager ..|> ISessionManager : implements
    
    WindowsProtectedDataProvider ..|> IProtectedDataProvider : implements
    LinuxProtectedDataProvider ..|> IProtectedDataProvider : implements
    ProtectedDataProviderFactory --> IProtectedDataProvider : creates
    
    SessionManager --> SessionSettings : contains
    SessionManager --> IProtectedDataProvider : uses
    SessionSettings --> Languages : uses
    
    GitHubModelCollection --> GitHubModel : aggregates
    GitHubModel --> GitHubModelLimits : contains
    
    TranslateCommand --> LoadingIndicatorExtensions : uses
    ModelsListCommand --> LoadingIndicatorExtensions : uses
    LoadingIndicatorExtensions --> LoadingIndicator : creates
    
    SetShowCommand --> StringExtensions : uses
    ModelsListCommand --> GitHubModelExtensions : uses
    TranslateCommand --> ConsoleUtility : uses
    TranslateCommand --> PromptFileUtility : uses
    
    %% Styling
    classDiagram
        class Program fill:#e8f4fd,stroke:#1976d2,stroke-width:2px
        class CommandBase fill:#fff3e0,stroke:#f57c00,stroke-width:2px
        class ISessionManager fill:#e8f5e8,stroke:#388e3c,stroke-width:2px
        class IModelsService fill:#e8f5e8,stroke:#388e3c,stroke-width:2px
        class IModelsClient fill:#e8f5e8,stroke:#388e3c,stroke-width:2px
        class IProtectedDataProvider fill:#e8f5e8,stroke:#388e3c,stroke-width:2px
        class SessionManager fill:#f3e5f5,stroke:#7b1fa2,stroke-width:2px
        class GitHubModelsService fill:#f3e5f5,stroke:#7b1fa2,stroke-width:2px
        class GitHubModelsClient fill:#f3e5f5,stroke:#7b1fa2,stroke-width:2px
```

---

## 🔒 Security

- Credentials are stored securely using platform-specific encryption
- GitHub tokens are masked in display output
- Settings can be completely cleared when needed

---

## 📄 License

See the main project LICENSE file for licensing information.