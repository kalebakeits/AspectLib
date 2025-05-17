using System.Reflection;
using AspectLib.Aspects.Caching;
using AspectLib.Aspects.Caching.CachingBackend;
using Moq;

namespace AspectLib.Weaving.UnitTest;

public class CacheAttributeWeavingTest : BaseWeavingTest
{
    [Fact(DisplayName = "CacheAttribute Weaves")]
    public void CacheAttribute_Weaves()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("key");
        this.mockCachingBackend.Setup(mock => mock.GetAsync(It.IsAny<string>(), It.IsAny<Type>()))
            .ReturnsAsync((object?)null);

        [Cache]
        static int method()
        {
            return 0;
        }

        // Act
        var result = method();

        // Assert
        Assert.Equal(0, result);
        this.mockCacheKeyFactory.Verify(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        );
        this.mockCachingBackend.Verify(mock => mock.GetAsync(It.IsAny<string>(), It.IsAny<Type>()));
        this.mockCachingBackend.Verify(
            mock =>
                mock.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<CachingBackendEntryOptions>()
                )
        );
    }
}
