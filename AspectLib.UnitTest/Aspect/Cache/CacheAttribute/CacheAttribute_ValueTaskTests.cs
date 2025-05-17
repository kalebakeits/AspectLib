namespace AspectLib.UnitTest.Aspcect.Cache.CacheAttribute;

using System.Reflection;
using System.Threading.Tasks;
using AspectLib.Aspects.Caching;
using AspectLib.Aspects.Caching.CachingBackend;
using Moq;

[Collection("CacheAttributeTests")]
public class CacheAttribute_ValueTaskTests : BaseCacheAttributeTests
{
    [Fact(DisplayName = "Advised ValueTask - Retrieves Result From Cache")]
    public async Task FromCache()
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
        ValueTask<int> method()
        {
            this.DummyField++;
            return new ValueTask<int>(1000);
        }

        // Act
        var result = await method();

        // Assert
        Assert.IsType<int>(result);
        Assert.Equal(1000, result);
        this.mockCachingBackend.Verify(mock => mock.GetAsync("key", typeof(int)));
    }

    [Fact(DisplayName = "Advised ValueTask - Retrieves Result From Context")]
    public async Task FromContext()
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
        ValueTask<int> method()
        {
            this.DummyField++;
            return new ValueTask<int>(1000);
        }

        // Act
        var result = await method();

        // Assert
        Assert.IsType<int>(result);
        Assert.Equal(1000, result);
        this.mockCachingBackend.Verify(mock => mock.GetAsync("key", typeof(int)));
        this.mockCachingBackend.Verify(
            mock => mock.SetAsync("key", 1000, It.IsAny<CachingBackendEntryOptions>())
        );
    }

    [Fact(DisplayName = "Advised ValueTask - Returns Interface Type From Cache")]
    public async Task Returns_Interface()
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
        ValueTask<ICustomInstanceInterface> method()
        {
            this.DummyField++;
            ICustomInstanceInterface customInterface = new CustomInstanceClass() { Number = 42 };
            return new ValueTask<ICustomInstanceInterface>(customInterface);
        }

        // Act
        var result = await method();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ICustomInstanceInterface>(result);
        Assert.IsType<CustomInstanceClass>(result);
        Assert.Equal(42, result.Number);
        this.mockCachingBackend.Verify(
            mock => mock.GetAsync("keyInterface", typeof(ICustomInstanceInterface))
        );
    }

    [Fact(DisplayName = "Advised ValueTask - Returns Complex Class Type")]
    public async Task Returns_CustomClass()
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
        ValueTask<CustomInstanceClass> method()
        {
            this.DummyField++;
            return new ValueTask<CustomInstanceClass>(new CustomInstanceClass { Number = 42 });
        }

        // Act
        var result = await method();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CustomInstanceClass>(result);
        Assert.Equal(42, result.Number);
        this.mockCachingBackend.Verify(
            mock => mock.GetAsync("keyCustomClass", typeof(CustomInstanceClass))
        );
    }

    [Fact(
        DisplayName = "Advised ValueTask - Returns Interface Collection Type (IDictionary<string,string>)"
    )]
    public async Task Returns_InterfaceCollectionType()
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
        ValueTask<IDictionary<string, string>> method()
        {
            this.DummyField++;
            IDictionary<string, string> dict = new Dictionary<string, string> { ["key"] = "value" };
            return new ValueTask<IDictionary<string, string>>(dict);
        }

        // Act
        var result = await method();

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
        DisplayName = "Advised ValueTask - Returns Concrete Collection Type (Dictionary<string,string>)"
    )]
    public async Task Returns_ConcreteCollectionType()
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
        ValueTask<Dictionary<string, string>> method()
        {
            this.DummyField++;
            return new ValueTask<Dictionary<string, string>>(
                new Dictionary<string, string> { ["key"] = "value" }
            );
        }

        // Act
        var result = await method();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Dictionary<string, string>>(result);
        Assert.Equal("value", result["key"]);
        this.mockCachingBackend.Verify(
            mock => mock.GetAsync("keyDictionary", typeof(Dictionary<string, string>))
        );
    }

    [Fact(DisplayName = "Advised ValueTask - Throws Exception and Does Not Cache")]
    public async Task Throws_Exception_DoesNotCache()
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
        ValueTask<int> method()
        {
            this.DummyField++;
            throw new InvalidOperationException("Test exception");
        }

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await method());

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
