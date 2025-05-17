namespace AspectLib.Aspects.Caching.CachingBackend;

/// <summary>
/// Provides methods to interact with a caching system.
/// </summary>
public interface ICachingBackend
{
    /// <summary>
    /// Retrieves a value from the cache.
    /// </summary>
    /// <param name="key">The unique key of the value to retrieve.</param>
    /// <param name="returnType">The type of the value to retrieve.</param>
    /// <returns>The value associated with the provided key.</returns>
    Task<object?> GetAsync(string key, Type returnType);

    /// <summary>
    /// Stores a value in the cache.
    /// </summary>
    /// <param name="key">The unique key of the value to store.</param>
    /// <param name="value">The value to store.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <param name="options">The options for the cache entry.</param>
    Task SetAsync(string key, object? value, CachingBackendEntryOptions? options = null);
}
