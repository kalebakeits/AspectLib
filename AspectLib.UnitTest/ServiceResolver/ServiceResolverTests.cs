namespace AspectLib.UnitTest.ServiceResolver;

using AspectLib.Aspects.Caching.CacheKeys;
using AspectLib.Aspects.Caching.CachingBackend;
using AspectLib.ServiceResolver;
using Xunit;

[Collection("Service Resolver Tests")]
public class ServiceResolverTests
{
    private readonly ServiceResolver resolver = new();

    [Fact(DisplayName = "Resolve returns CacheKeyService for ICacheKeyFactory")]
    public void Resolve_ReturnsCacheKeyService_ForICacheKeyFactory()
    {
        var result = resolver.Resolve<ICacheKeyFactory>();
        Assert.NotNull(result);
        Assert.IsType<CacheKeyService>(result);
    }

    [Fact(DisplayName = "Resolve returns CachingBackend for ICachingBackend")]
    public void Resolve_ReturnsCachingBackend_ForICachingBackend()
    {
        var result = resolver.Resolve<ICachingBackend>();
        Assert.NotNull(result);
        Assert.IsType<CachingBackend>(result);
    }

    [Fact(DisplayName = "Resolve returns null for unknown service")]
    public void Resolve_ReturnsNull_ForUnknownService()
    {
        var result = resolver.Resolve<IServiceResolver>();
        Assert.Null(result);
    }
}
