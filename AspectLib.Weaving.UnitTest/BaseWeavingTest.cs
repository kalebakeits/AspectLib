namespace AspectLib.Weaving.UnitTest;

using AspectLib.Aspects.Caching.CacheKeys;
using AspectLib.Aspects.Caching.CachingBackend;
using AspectLib.ServiceResolver;
using Moq;

public class BaseWeavingTest
{
    protected readonly Mock<IServiceResolver> mockServiceResolver = new();
    protected readonly Mock<ICachingBackend> mockCachingBackend = new();
    protected readonly Mock<ICacheKeyFactory> mockCacheKeyFactory = new();

    public BaseWeavingTest()
    {
        mockServiceResolver
            .Setup(mock => mock.Resolve<ICachingBackend>())
            .Returns(mockCachingBackend.Object);
        mockServiceResolver
            .Setup(mock => mock.Resolve<ICacheKeyFactory>())
            .Returns(mockCacheKeyFactory.Object);
        AspectServiceResolver.Resolver = mockServiceResolver.Object;
    }
}
