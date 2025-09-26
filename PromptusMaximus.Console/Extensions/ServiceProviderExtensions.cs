
using GitHubModel.Core.Interfaces;
using PromptusMaximus.Core.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ServiceProviderExtensions
{
    public static IModelsClient GetModelsClient(this ServiceProvider provider)
    {
        return provider.GetRequiredService<IModelsClient>();
    }

    public static IModelsService GetModelsService(this ServiceProvider provider)
    {
        return provider.GetRequiredService<IModelsService>();
    }

    public static ISessionManager GetSessionManager(this ServiceProvider provider)
    {
        return provider.GetRequiredService<ISessionManager>();
    }
}
