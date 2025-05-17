namespace AspectLib.Aspects.Caching.CachingBackend;

/// <summary>
/// Options for a single caching backend entry.
/// </summary>
public class CachingBackendEntryOptions
{
    /// <summary>
    /// Absolute expiration relative to now. Null means no absolute expiration.
    /// </summary>
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    /// Sliding expiration. Resets expiration timer on each access.
    /// </summary>
    public TimeSpan? SlidingExpiration { get; set; }

    /// <summary>
    /// Priority for cache eviction.
    /// </summary>
    public CachePriority Priority { get; set; } = CachePriority.Normal;

    /// <summary>
    /// Region or partition name for grouping cache entries.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Indicates if the cached value should be compressed.
    /// </summary>
    public bool EnableCompression { get; set; } = false;

    /// <summary>
    /// Threshold size in bytes above which compression is applied.
    /// </summary>
    public int CompressionThresholdBytes { get; set; } = 1024 * 10; // 10 KB default

    /// <summary>
    /// Flag to indicate whether to cache null results.
    /// </summary>
    public bool CacheNullValues { get; set; } = false;

    /// <summary>
    /// Whether the cached entry can be refreshed in the background before expiration.
    /// </summary>
    public bool EnableBackgroundRefresh { get; set; } = false;

    /// <summary>
    /// Threshold percentage (0-100) of TTL elapsed to trigger background refresh.
    /// </summary>
    public int BackgroundRefreshThresholdPercent { get; set; } = 80;

    /// <summary>
    /// Whether to track cache hit/miss metrics for this entry.
    /// </summary>
    public bool EnableMetrics { get; set; } = false;

    /// <summary>
    /// Tags or labels for monitoring and categorization.
    /// </summary>
    public string[]? Tags { get; set; }

    /// <summary>
    /// Custom serializer identifier or name to use for this entry.
    /// </summary>
    public string? SerializerName { get; set; }
}
