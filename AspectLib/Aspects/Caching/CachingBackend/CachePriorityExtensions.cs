namespace AspectLib.Aspects.Caching.CachingBackend;

using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Extensions for <see cref="CachePriority"/>.
/// </summary>
public static class CachePriorityExtensions
{
    /// <summary>
    /// Maps <see cref="CachePriority"/> to <see cref="CacheItemPriority"/>.
    /// </summary>
    /// <param name="priority">The priority.</param>
    /// <returns><see cref="CacheItemPriority"/>.</returns>
    public static CacheItemPriority MapToMemoryCacheItemPriotity(this CachePriority priority) =>
        priority switch
        {
            CachePriority.Low => CacheItemPriority.Low,
            CachePriority.Normal => CacheItemPriority.Normal,
            CachePriority.High => CacheItemPriority.High,
            CachePriority.NeverRemove => CacheItemPriority.NeverRemove,
            _ => CacheItemPriority.Normal
        };
}
