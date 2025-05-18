namespace AspectLib.TestHarness.Features.Dummy;

/// <summary>
/// Configure the dummy service feature.
/// </summary>
public static class DummyFeature
{
    /// <summary>
    /// Configures the dummy service feature.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static IServiceCollection AddDummyFeature(this IServiceCollection services) =>
        services.AddSingleton<IDummyService, DummyService>();
}
