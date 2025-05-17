namespace AspectLib.ServiceResolver;

/// <summary>
/// Provides a static service resolver interface for resolving services.
/// </summary>
public static class AspectServiceResolver
{
    /// <summary>
    /// Gets or sets the current <see cref="IServiceResolver"/> instance.
    /// </summary>
    public static IServiceResolver? Resolver { get; set; } = null;

    /// <summary>
    /// Resolves a service of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of service to resolve.</typeparam>
    /// <returns>The resolved service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service could not be resolved.</exception>
    public static T Resolve<T>()
        where T : class =>
        Resolver?.Resolve<T>()
        ?? DefaultResolver.Resolve<T>()
        ?? throw new InvalidOperationException(
            $"Could not resolve service for {typeof(T).FullName}."
        );

    private static ServiceResolver DefaultResolver { get; } = new();
}
