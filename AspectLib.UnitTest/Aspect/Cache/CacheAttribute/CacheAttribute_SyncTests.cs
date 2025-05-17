namespace AspectLib.UnitTest.Aspcect.Cache.CacheAttribute;

using System.Reflection;
using AspectLib.Aspects.Caching;
using AspectLib.Aspects.Caching.CachingBackend;
using Moq;

[Collection("CacheAttributeTests")]
public class CacheAttribute_SyncTests : BaseCacheAttributeTests
{
    [Fact(DisplayName = "Advised Sync - Retrieves Result From Cache")]
    public void FromCache()
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
        this.mockCachingBackend.Setup(mock => mock.GetAsync(It.IsAny<string>(), typeof(int)))
            .ReturnsAsync(1000);

        [Cache]
        int method()
        {
            this.DummyField++;
            return 1000;
        }

        // Act
        var result = method();

        // Assert
        Assert.IsType<int>(result);
        Assert.Equal(1000, result);
        this.mockCachingBackend.Verify(mock => mock.GetAsync("key", typeof(int)));
    }

    [Fact(DisplayName = "Advised Sync - Retrieves Result From Context")]
    public void FromContext()
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
        this.mockCachingBackend.Setup(mock => mock.GetAsync(It.IsAny<string>(), typeof(int)))
            .ReturnsAsync((object?)null);

        [Cache]
        int method()
        {
            this.DummyField++;
            return 1000;
        }

        // Act
        var result = method();

        // Assert
        Assert.IsType<int>(result);
        Assert.Equal(1000, result);
        this.mockCachingBackend.Verify(mock => mock.GetAsync("key", typeof(int)));
        this.mockCachingBackend.Verify(
            mock => mock.SetAsync("key", 1000, It.IsAny<CachingBackendEntryOptions>())
        );
    }

    [Fact(DisplayName = "Advised Sync - Returns Interface Type From Cache")]
    public void Returns_Interface()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("keyInterface");

        var cachedValue = new CustomInstanceClass() { Number = 42 };
        this.mockCachingBackend.Setup(
            mock => mock.GetAsync("keyInterface", typeof(ICustomInstanceInterface))
        )
            .ReturnsAsync(cachedValue);

        [Cache]
        ICustomInstanceInterface method()
        {
            this.DummyField++;
            ICustomInstanceInterface customInterface = new CustomInstanceClass() { Number = 42 };
            return customInterface;
        }

        // Act
        var result = method();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ICustomInstanceInterface>(result);
        Assert.IsType<CustomInstanceClass>(result);
        Assert.Equal(42, result.Number);
        this.mockCachingBackend.Verify(
            mock => mock.GetAsync("keyInterface", typeof(ICustomInstanceInterface))
        );
    }

    [Fact(DisplayName = "Advised Sync - Returns Complex Class Type")]
    public void Returns_CustomClass()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("keyCustomClass");

        var cachedValue = new CustomInstanceClass { Number = 42 };
        this.mockCachingBackend.Setup(
            mock => mock.GetAsync("keyCustomClass", typeof(CustomInstanceClass))
        )
            .ReturnsAsync(cachedValue);

        [Cache]
        CustomInstanceClass method()
        {
            this.DummyField++;
            return new CustomInstanceClass { Number = 42 };
        }

        // Act
        var result = method();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CustomInstanceClass>(result);
        Assert.Equal(42, result.Number);
        this.mockCachingBackend.Verify(
            mock => mock.GetAsync("keyCustomClass", typeof(CustomInstanceClass))
        );
    }

    [Fact(
        DisplayName = "Advised Sync - Returns Interface Collection Type (IDictionary<string,string>)"
    )]
    public void Returns_InterfaceCollectionType()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("keyIDictionary");

        var cachedValue = new Dictionary<string, string> { ["key"] = "value" };
        this.mockCachingBackend.Setup(
            mock => mock.GetAsync("keyIDictionary", typeof(IDictionary<string, string>))
        )
            .ReturnsAsync(cachedValue);

        [Cache]
        IDictionary<string, string> method()
        {
            this.DummyField++;
            IDictionary<string, string> dict = new Dictionary<string, string> { ["key"] = "value" };
            return dict;
        }

        // Act
        var result = method();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IDictionary<string, string>>(result);
        Assert.IsType<Dictionary<string, string>>(result);
        Assert.Equal("value", result["key"]);
        this.mockCachingBackend.Verify(
            mock => mock.GetAsync("keyIDictionary", typeof(IDictionary<string, string>))
        );
    }

    [Fact(
        DisplayName = "Advised Sync - Returns Concrete Collection Type (Dictionary<string,string>)"
    )]
    public void Returns_ConcreteCollectionType()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("keyDictionary");

        var cachedValue = new Dictionary<string, string> { ["key"] = "value" };
        this.mockCachingBackend.Setup(
            mock => mock.GetAsync("keyDictionary", typeof(Dictionary<string, string>))
        )
            .ReturnsAsync(cachedValue);

        [Cache]
        Dictionary<string, string> method()
        {
            this.DummyField++;
            return new Dictionary<string, string> { ["key"] = "value" };
        }

        // Act
        var result = method();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Dictionary<string, string>>(result);
        Assert.Equal("value", result["key"]);
        this.mockCachingBackend.Verify(
            mock => mock.GetAsync("keyDictionary", typeof(Dictionary<string, string>))
        );
    }

    [Fact(DisplayName = "Advised Sync - Throws Exception and Does Not Cache")]
    public void Throws_Exception_DoesNotCache()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("keyException");

        [Cache]
        int method()
        {
            this.DummyField++;
            throw new InvalidOperationException("Test exception");
        }

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => method());

        Assert.Equal("Test exception", ex.Message);
        this.mockCachingBackend.Verify(
            mock => mock.GetAsync(It.IsAny<string>(), It.IsAny<Type>()),
            Times.Once
        );
        this.mockCachingBackend.Verify(
            mock =>
                mock.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<CachingBackendEntryOptions>()
                ),
            Times.Never()
        );
    }
}
