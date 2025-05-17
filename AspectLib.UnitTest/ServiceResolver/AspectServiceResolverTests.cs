namespace AspectLib.UnitTest.ServiceResolver;

using AspectLib.ServiceResolver;
using Moq;

[Collection("Service Resolver Tests")]
public class AspectServiceResolverTests
{
    private readonly Mock<IServiceResolver> mockResolver;

    public AspectServiceResolverTests()
    {
        mockResolver = new Mock<IServiceResolver>();
        AspectServiceResolver.Resolver = mockResolver.Object;
    }

    [Fact(DisplayName = "Resolve<T> returns resolved service instance")]
    public void Resolve_ReturnsResolvedInstance()
    {
        var expected = new DummyService();
        mockResolver.Setup(r => r.Resolve<DummyService>()).Returns(expected);
        AspectServiceResolver.Resolver = mockResolver.Object;

        var actual = AspectServiceResolver.Resolve<DummyService>();

        Assert.Same(expected, actual);
        mockResolver.Verify(r => r.Resolve<DummyService>(), Times.Once);
    }

    [Fact(DisplayName = "Resolve<T> throws if service not resolved")]
    public void Resolve_ThrowsIfNull()
    {
        mockResolver.Setup(r => r.Resolve<DummyService>()).Returns((DummyService?)null);

        var ex = Assert.Throws<InvalidOperationException>(
            AspectServiceResolver.Resolve<DummyService>
        );

        Assert.Contains("Could not resolve service", ex.Message);
    }

    private class DummyService { }
}
