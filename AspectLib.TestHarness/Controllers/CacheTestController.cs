namespace AspectLib.TestHarness.Controllers;

using AspectLib.TestHarness.Features.Dummy;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// A controller for testing caching.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CacheTestController"/> class.
/// </remarks>
/// <param name="logger">An instance of <see cref="ILogger{CacheTestController}"/>.</param>
/// <param name="dummyService">An instance of <see cref="IDummyService"/>.</param>
[Route("api/[controller]")]
[ApiController]
public class CacheTestController(ILogger<CacheTestController> logger, IDummyService dummyService)
    : ControllerBase
{
    private readonly ILogger<CacheTestController> logger = logger;
    private readonly IDummyService dummyService = dummyService;

    [HttpGet]
    public async Task<ActionResult<KeyValuePair<string, string>[]>> Get()
    {
        this.logger.LogInformation("Getting values");

        return (
            await Task.WhenAll(
                Enumerable.Range(1, 10).Select(i => this.dummyService.Get(i.ToString()))
            )
        )
            .SelectMany(x => x)
            .ToArray();
    }
}
