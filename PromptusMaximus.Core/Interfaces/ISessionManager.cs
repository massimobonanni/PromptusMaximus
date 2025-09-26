using PromptusMaximus.Core.Configuration;

namespace PromptusMaximus.Core.Interfaces;

/// <summary>
/// Defines the contract for managing user session settings, including configuration persistence and secret management.
/// </summary>
public interface ISessionManager
{
    /// <summary>
    /// Gets the current session settings containing model, language, and custom configurations.
    /// </summary>
    /// <value>The current <see cref="SessionSettings"/> instance with all loaded configuration values.</value>
    SessionSettings CurrentSettings { get; }

    /// <summary>
    /// Clears all session settings and optionally deletes the associated configuration files.
    /// </summary>
    /// <param name="deleteFiles">If <c>true</c>, deletes the configuration files from disk; otherwise, only clears in-memory settings. Default is <c>true</c>.</param>
    /// <returns>A task that represents the asynchronous clear operation.</returns>
    Task ClearAllSettingsAsync(bool deleteFiles = true);

    /// <summary>
    /// Retrieves the GitHub authentication token from the secure storage.
    /// </summary>
    /// <returns>The GitHub token if available; otherwise, <c>null</c>.</returns>
    string? GetGitHubToken();

    /// <summary>
    /// Retrieves a secret value by its key from the secure storage.
    /// </summary>
    /// <param name="key">The key identifying the secret value to retrieve.</param>
    /// <returns>The secret value if found; otherwise, <c>null</c>.</returns>
    string? GetSecret(string key);

    /// <summary>
    /// Retrieves a custom setting value by its key.
    /// </summary>
    /// <param name="key">The key identifying the setting value to retrieve.</param>
    /// <returns>The setting value if found; otherwise, <c>null</c>.</returns>
    string? GetSetting(string key);

    /// <summary>
    /// Loads session settings from persistent storage asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous load operation.</returns>
    Task LoadSettingsAsync();

    /// <summary>
    /// Saves the current session settings to persistent storage asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveSettingsAsync();

    /// <summary>
    /// Sets the GitHub authentication token in secure storage.
    /// </summary>
    /// <param name="token">The GitHub token to store securely.</param>
    void SetGitHubToken(string token);

    /// <summary>
    /// Sets the language preference for the current session.
    /// </summary>
    /// <param name="language">The language preference from the <see cref="Languages"/> enumeration.</param>
    void SetLanguage(Languages language);

    /// <summary>
    /// Sets the AI model identifier for the current session.
    /// </summary>
    /// <param name="model">The name or identifier of the AI model to use.</param>
    void SetModel(string model);

    /// <summary>
    /// Sets a secret value in secure storage, identified by the specified key.
    /// </summary>
    /// <param name="key">The key to identify the secret value.</param>
    /// <param name="value">The secret value to store securely.</param>
    void SetSecret(string key, string value);

    /// <summary>
    /// Sets a custom setting value, identified by the specified key.
    /// </summary>
    /// <param name="key">The key to identify the setting.</param>
    /// <param name="value">The setting value to store.</param>
    void SetSetting(string key, string value);
}