namespace AspectLib.ServiceResolver;

/// <summary>
/// Defines a contract for resolving services.
/// </summary>
public interface IServiceResolver
{
    /// <summary>
    /// Resolves an instance of the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type of the service to resolve.</param>
    /// <returns>The resolved service instance or <see langword="null"/> if not available.</returns>
    T? Resolve<T>()
        where T : class;
}
