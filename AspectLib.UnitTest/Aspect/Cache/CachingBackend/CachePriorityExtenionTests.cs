namespace AspectLib.UnitTest.Aspcect.Cache.CachingBackend;

using AspectLib.Aspects.Caching.CachingBackend;
using Microsoft.Extensions.Caching.Memory;

[Collection("CachingBackendTests")]
public class CachePriorityExtenionTests
{
    [Theory(DisplayName = "MapToMemoryCacheItemPriotity - Maps to correct value")]
    [InlineData(CachePriority.Low, CacheItemPriority.Low)]
    [InlineData(CachePriority.Normal, CacheItemPriority.Normal)]
    [InlineData(CachePriority.High, CacheItemPriority.High)]
    [InlineData(CachePriority.NeverRemove, CacheItemPriority.NeverRemove)]
    public void MapToMemoryCacheItemPriotity(CachePriority priority, CacheItemPriority expected) =>
        Assert.Equal(expected, priority.MapToMemoryCacheItemPriotity());
}
