namespace AspectLib.Aspects.Caching.CachingBackend;

using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// The default caching backend provided by AspectLib, using MemoryCache.
/// </summary>
public class CachingBackend : ICachingBackend
{
    private static readonly MemoryCache _cache = new(new MemoryCacheOptions());

    /// <inheritdoc/>
    public Task<object?> GetAsync(string key, Type returnType)
    {
        _cache.TryGetValue(key, out var value);
        return Task.FromResult(value);
    }

    /// <inheritdoc/>
    public Task SetAsync(string key, object? value, CachingBackendEntryOptions? options = null)
    {
        var cacheOptions = new MemoryCacheEntryOptions();

        if (options != null)
        {
            if (options.AbsoluteExpirationRelativeToNow.HasValue)
                cacheOptions.SetAbsoluteExpiration(options.AbsoluteExpirationRelativeToNow.Value);

            if (options.SlidingExpiration.HasValue)
                cacheOptions.SetSlidingExpiration(options.SlidingExpiration.Value);

            cacheOptions.SetPriority(options.Priority.MapToMemoryCacheItemPriotity());
        }

        _cache.Set(key, value, cacheOptions);
        return Task.CompletedTask;
    }
}
