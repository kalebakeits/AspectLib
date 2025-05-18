namespace AspectLib.TestHarness.Features.Cache;

using System.Reflection;
using AspectLib.Aspects.Caching.CacheKeys;

/// <summary>
/// Custom cache key factory for testing.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CacheKeyFactory"/> class.
/// </remarks>
/// <param name="logger">An instance of <see cref="ILogger{CacheKeyService}"/>.</param>
public class CacheKeyFactory(ILogger<CacheKeyService> logger) : ICacheKeyFactory
{
    private readonly ILogger<CacheKeyService> logger = logger;

    /// <inheritdoc/>
    public string GenerateCacheKey(string template, KeyValuePair<string, object>[] args)
    {
        this.logger.LogInformation("Using custom cache key factory");
        return template;
    }

    /// <inheritdoc/>
    public string GenerateCacheKey(MethodInfo method, KeyValuePair<string, object>[] args)
    {
        this.logger.LogInformation("Using custom cache key factory");
        return method.Name;
    }
}
