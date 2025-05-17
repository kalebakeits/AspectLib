using System.Reflection;

namespace AspectLib.Aspects.Caching.CacheKeys;

/// <summary>
/// Generates cache keys.
/// </summary>
public interface ICacheKeyFactory
{
    /// <summary>
    /// Generates the cache key based on the template.
    /// </summary>
    /// <param name="template">The cache key template.</param>
    /// <param name="args">The arguemnts passed to the method.</param>
    /// <returns>The cache key.</returns>
    string GenerateCacheKey(string template, params KeyValuePair<string, object>[] args);

    /// <summary>
    /// Generates the default cache key based on the method name and arguments.
    /// </summary>
    /// <param name="methodInfo">The method info.</param>
    /// <param name="args">The arguemnts passed to the method.</param>
    /// <returns>The cache key.</returns>
    string GenerateCacheKey(MethodInfo methodInfo, params KeyValuePair<string, object>[] args);
}
