namespace AspectLib.Aspects.Caching.CachingBackend;

using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Priority for cache eviction.
/// </summary>
public enum CachePriority : int
{
    Low = 0,
    Normal = 10,
    High = 50,
    NeverRemove = 100
}
