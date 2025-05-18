namespace AspectLib.TestHarness.Features.Dummy;

/// <summary>
/// A dummy service for testing.
/// </summary>
public interface IDummyService
{
    /// <summary>
    /// A dummy method for testing.
    /// </summary>
    /// <param name="value">A string.</param>
    /// <returns>A string.</returns>
    Task<Dictionary<string, string>> Get(string value);
}
