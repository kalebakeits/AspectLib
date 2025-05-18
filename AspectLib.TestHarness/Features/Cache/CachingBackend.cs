namespace AspectLib.TestHarness.Features.Cache;

using System.Text;
using System.Text.Json;
using AspectLib.Aspects.Caching.CachingBackend;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// Custom caching backend for testing.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CachingBackend"/> class.
/// </remarks>
/// <param name="cache">An instance of <see cref="IDistributedCache"/>.</param><param name="logger">An instance of <see cref="ILogger{CachingBackend}"/>.</param>
/// <param name="logger">An instance of <see cref="ILogger{CachingBackend}"/>.</param>
public class CachingBackend(IDistributedCache cache, ILogger<CachingBackend> logger)
    : ICachingBackend
{
    private readonly IDistributedCache cache = cache;
    private readonly ILogger<CachingBackend> logger = logger;
    private readonly JsonSerializerOptions jsonOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <inheritdoc/>
    public async Task SetAsync(
        string key,
        object? value,
        CachingBackendEntryOptions? options = null
    )
    {
        this.logger.LogInformation("Setting cache entry {key}", key);
        DistributedCacheEntryOptions cacheEntryOptions =
            new()
            {
                AbsoluteExpirationRelativeToNow = options?.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = options?.SlidingExpiration,
            };
        string json = JsonSerializer.Serialize(value, this.jsonOptions);
        await this.cache.SetAsync(key, Encoding.UTF8.GetBytes(json), cacheEntryOptions);
    }

    /// <inheritdoc/>
    public async Task<object?> GetAsync(string key, Type type)
    {
        this.logger.LogInformation("Getting cache entry {key}", key);
        byte[]? json = await cache.GetAsync(key);
        if (json is null)
        {
            return null;
        }
        return JsonSerializer.Deserialize(json, type, this.jsonOptions);
    }
}
