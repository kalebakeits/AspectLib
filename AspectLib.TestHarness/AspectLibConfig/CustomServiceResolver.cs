namespace AspectLib.TestHarness.AspectLibConfig;

using AspectLib.ServiceResolver;

/// <summary>
/// A custom service resolver implementation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ServiceResolver"/> class.
/// </remarks>
/// <param name="serviceProvider">An instance of <see cref="IServiceProvider"/>.</param>
public class CustomServiceResolver(IServiceProvider serviceProvider) : IServiceResolver
{
    private readonly IServiceProvider serviceProvider = serviceProvider;

    /// <inheritdoc/>
    public T? Resolve<T>()
        where T : class
    {
        IHttpContextAccessor accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        return accessor.HttpContext?.RequestServices.GetService<T>();
    }
}
