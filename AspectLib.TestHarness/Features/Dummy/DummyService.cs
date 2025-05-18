namespace AspectLib.TestHarness.Features.Dummy;

using AspectLib.Aspects.Caching;

/// <summary>
/// A dummy service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DummyService"/> class.
/// </remarks>
/// <param name="logger">An instance of <see cref="ILogger{DummyService}"/>.</param>
public class DummyService(ILogger<DummyService> logger) : IDummyService
{
    private readonly ILogger<DummyService> logger = logger;

    /// <inheritdoc/>
    public async Task<Dictionary<string, string>> Get(string value)
    {
        this.logger.LogInformation("Getting value {value}", value);
        return (await GetCached1(value)).ToDictionary();
    }

    [Cache]
    private async Task<IDictionary<string, string>> GetCached1(string value)
    {
        value = $"value_{value}";
        this.logger.LogInformation("Did not use cache for {value}", value);
        return await Task.FromResult(new Dictionary<string, string> { { value, value } });
    }
}
