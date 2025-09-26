﻿using GitHubModel.Core.Interfaces;
using PromptusMaximus.Core.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for <see cref="ServiceProvider"/> to simplify service retrieval for PromptusMaximus application services.
/// </summary>
internal static class ServiceProviderExtensions
{
    /// <summary>
    /// Retrieves the registered <see cref="IModelsClient"/> service from the service provider.
    /// </summary>
    /// <param name="provider">The service provider to retrieve the service from.</param>
    /// <returns>The registered <see cref="IModelsClient"/> implementation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the <see cref="IModelsClient"/> service is not registered in the service provider.</exception>
    public static IModelsClient GetModelsClient(this ServiceProvider provider)
    {
        return provider.GetRequiredService<IModelsClient>();
    }

    /// <summary>
    /// Retrieves the registered <see cref="IModelsService"/> service from the service provider.
    /// </summary>
    /// <param name="provider">The service provider to retrieve the service from.</param>
    /// <returns>The registered <see cref="IModelsService"/> implementation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the <see cref="IModelsService"/> service is not registered in the service provider.</exception>
    public static IModelsService GetModelsService(this ServiceProvider provider)
    {
        return provider.GetRequiredService<IModelsService>();
    }

    /// <summary>
    /// Retrieves the registered <see cref="ISessionManager"/> service from the service provider.
    /// </summary>
    /// <param name="provider">The service provider to retrieve the service from.</param>
    /// <returns>The registered <see cref="ISessionManager"/> implementation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the <see cref="ISessionManager"/> service is not registered in the service provider.</exception>
    public static ISessionManager GetSessionManager(this ServiceProvider provider)
    {
        return provider.GetRequiredService<ISessionManager>();
    }
}
