using AspectLib.Aspects.Caching.CacheKeys;
using AspectLib.Aspects.Caching.CachingBackend;

namespace AspectLib.ServiceResolver;

/// <summary>
/// A default service resolver implementation that returns default service instances.
/// </summary>
public class ServiceResolver : IServiceResolver
{
    /// <inheritdoc />
    public virtual T? Resolve<T>()
        where T : class
    {
        if (typeof(T) == typeof(ICacheKeyFactory))
            return new CacheKeyService() as T;
        if (typeof(T) == typeof(ICachingBackend))
            return new CachingBackend() as T;
        return null;
    }
}
