namespace AspectLib.TestHarness.DependencyInjection;

using AspectLib.TestHarness.Features.Cache;
using AspectLib.TestHarness.Features.Dummy;

/// <summary>
/// Configure services on the DI container
/// </summary>
public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services.AddDummyFeature().AddCacheFeature();
    }
}
