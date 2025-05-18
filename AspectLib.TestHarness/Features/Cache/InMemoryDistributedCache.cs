namespace AspectLib.TestHarness.Features.Cache;

using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// An in-memory implementation of IDistributedCache.
/// </summary>
public class InMemoryDistributedCache : IDistributedCache
{
    private readonly ConcurrentDictionary<string, byte[]> _cache = new();

    /// <inheritdoc/>
    public byte[]? Get(string key) => _cache.TryGetValue(key, out var value) ? value : null;

    /// <inheritdoc/>
    public Task<byte[]?> GetAsync(string key, CancellationToken token = default) =>
        Task.FromResult(Get(key));

    /// <inheritdoc/>
    public void Refresh(string key)
    {
        // No expiration logic in this simple cache
    }

    /// <inheritdoc/>
    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        Refresh(key);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public void Remove(string key)
    {
        _cache.TryRemove(key, out _);
    }

    /// <inheritdoc/>
    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        _cache[key] = value;
        // No expiration handling here for simplicity
    }

    /// <inheritdoc/>
    public Task SetAsync(
        string key,
        byte[] value,
        DistributedCacheEntryOptions options,
        CancellationToken token = default
    )
    {
        Set(key, value, options);
        return Task.CompletedTask;
    }
}
