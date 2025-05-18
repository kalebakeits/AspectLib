namespace AspectLib.TestHarness.Features.Cache;

using AspectLib.Aspects.Caching.CacheKeys;
using AspectLib.Aspects.Caching.CachingBackend;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// Configure the dummy service feature.
/// </summary>
public static class DummyServiceFeature
{
    /// <summary>
    /// Configures the dummy service feature.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static IServiceCollection AddCacheFeature(this IServiceCollection services) =>
        services
            .AddSingleton<ICacheKeyFactory, CacheKeyFactory>()
            .AddTransient<ICachingBackend, CachingBackend>()
            .AddSingleton<IDistributedCache, InMemoryDistributedCache>();
}
