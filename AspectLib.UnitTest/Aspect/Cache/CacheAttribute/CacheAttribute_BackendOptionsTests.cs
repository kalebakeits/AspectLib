namespace AspectLib.UnitTest.Aspcect.Cache.CacheAttribute;

using System.Reflection;
using AspectLib.Aspects.Caching;
using AspectLib.Aspects.Caching.CachingBackend;
using Moq;

[Collection("CacheAttributeTests")]
public class CacheAttribute_BackendOptionsTests : BaseCacheAttributeTests
{
    [Fact(DisplayName = "Backend Options - Passed to CachingBackend correctly")]
    public async Task BackendOptionsPassedCorrectly()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            m =>
                m.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("key");

        this.mockCachingBackend.Setup(m => m.GetAsync("key", typeof(int)))
            .ReturnsAsync((object?)null);

        var expected = new CachingBackendEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(600),
            SlidingExpiration = TimeSpan.FromSeconds(300),
            Priority = CachePriority.High,
            EnableCompression = true,
            CompressionThresholdBytes = 2048,
            CacheNullValues = true,
            EnableBackgroundRefresh = true,
            BackgroundRefreshThresholdPercent = 75,
            EnableMetrics = true,
            SerializerName = "CustomSerializer"
        };

        var optionsCapture = new CachingBackendEntryOptions();

        this.mockCachingBackend.Setup(
            m => m.SetAsync("key", 1000, It.IsAny<CachingBackendEntryOptions>())
        )
            .Callback<string, object, CachingBackendEntryOptions>(
                (k, v, opts) =>
                {
                    optionsCapture = opts;
                }
            );

        // Act
        await Method();

        // Assert
        Assert.Equivalent(expected, optionsCapture);
    }

    [Fact(DisplayName = "Backend Options - Defaults are applied correctly")]
    public async Task BackendOptionsDefaultsApplied()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            m =>
                m.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("key");

        this.mockCachingBackend.Setup(m => m.GetAsync("key", typeof(int)))
            .ReturnsAsync((object?)null);

        var expected = new CachingBackendEntryOptions
        {
            Priority = CachePriority.Normal,
            CompressionThresholdBytes = 10240,
            BackgroundRefreshThresholdPercent = 80
        };

        var optionsCapture = new CachingBackendEntryOptions();

        this.mockCachingBackend.Setup(
            m => m.SetAsync("key", 1000, It.IsAny<CachingBackendEntryOptions>())
        )
            .Callback<string, object, CachingBackendEntryOptions>(
                (k, v, opts) =>
                {
                    optionsCapture = opts;
                }
            );

        [Cache]
        Task<int> method()
        {
            this.DummyField++;
            return Task.FromResult(1000);
        }

        // Act
        await method();

        // Assert
        Assert.Equivalent(expected, optionsCapture);
    }

    [Cache(
        absoluteExpirationSeconds: 600,
        slidingExpirationSeconds: 300,
        priority: CachePriority.High,
        enableCompression: true,
        compressionThresholdBytes: 2048,
        cacheNullValues: true,
        enableBackgroundRefresh: true,
        backgroundRefreshThresholdPercent: 75,
        enableMetrics: true,
        serializerName: "CustomSerializer"
    )]
    /// <summary>
    /// Method. When passing options to the attribute the method will not be advised if is is a local method.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<int> Method()
    {
        this.DummyField++;
        return Task.FromResult(1000);
    }
}
