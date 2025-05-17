namespace AspectLib.UnitTest.Aspcect.Cache.CacheAttribute;

using System.Reflection;
using AspectLib.Aspects.Caching;
using Moq;

[Collection("CacheAttributeTests")]
public class CacheAttribute_KeyGenerationTests : BaseCacheAttributeTests
{
    [Fact(DisplayName = "Key Generation - Uses method name if template is null")]
    public async Task UsesMethodNameIfTemplateIsNull()
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
        Task<int> method()
        {
            this.DummyField++;
            return Task.FromResult(1000);
        }

        // Act
        await method();

        // Assert
        this.mockCacheKeyFactory.Verify(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<MethodInfo>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        );
    }

    [Fact(DisplayName = "Key Generation - Uses template if it is not null")]
    public async Task UsesTemplateIfNotNull()
    {
        // Arrange
        this.mockCacheKeyFactory.Setup(
            mock =>
                mock.GenerateCacheKey(
                    It.IsAny<string>(),
                    It.IsAny<KeyValuePair<string, object>[]>()
                )
        )
            .Returns("Template_replaced");

        [Cache("Template_{arg1}")]
        Task<int> method(string arg1)
        {
            this.DummyField++;
            return Task.FromResult(1000);
        }

        // Act
        await method("replaced");

        // Assert
        this.mockCacheKeyFactory.Verify(
            mock =>
                mock.GenerateCacheKey("Template_{arg1}", It.IsAny<KeyValuePair<string, object>[]>())
        );
    }
}
