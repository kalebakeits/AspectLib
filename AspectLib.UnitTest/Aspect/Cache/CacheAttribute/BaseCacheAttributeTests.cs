namespace AspectLib.UnitTest.Aspcect.Cache.CacheAttribute;

using AspectLib.Aspects.Caching.CacheKeys;
using AspectLib.Aspects.Caching.CachingBackend;
using AspectLib.ServiceResolver;
using Moq;

public class BaseCacheAttributeTests
{
    protected readonly Mock<ICachingBackend> mockCachingBackend = new();
    protected readonly Mock<ICacheKeyFactory> mockCacheKeyFactory = new();
    protected readonly Mock<IServiceResolver> mockServiceResolver = new();
    protected int DummyField { get; set; } = 0;

    public BaseCacheAttributeTests()
    {
        mockServiceResolver
            .Setup(mock => mock.Resolve<ICachingBackend>())
            .Returns(mockCachingBackend.Object);
        mockServiceResolver
            .Setup(mock => mock.Resolve<ICacheKeyFactory>())
            .Returns(mockCacheKeyFactory.Object);
        AspectServiceResolver.Resolver = mockServiceResolver.Object;
    }

    internal class CustomInstanceClass : ICustomInstanceInterface
    {
        public int Number { get; set; }
    }

    internal interface ICustomInstanceInterface
    {
        int Number { get; set; }
    }
}
