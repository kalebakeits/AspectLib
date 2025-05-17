namespace AspectLib.UnitTest.Aspcect.Cache.CachingBackend;

using AspectLib.Aspects.Caching.CachingBackend;

[Collection("CachingBackendTests")]
public class CachingBackendTests
{
    private readonly CachingBackend backend = new();

    [Fact(DisplayName = "GetAsync - Returns Null If Not Cached")]
    public async Task GetAsync_ReturnsNullIfNotCached()
    {
        var key = "nonexistent_key";
        var result = await backend.GetAsync(key, typeof(object));
        Assert.Null(result);
    }

    [Fact(DisplayName = "SetAsync - Stores Value And GetAsync Returns It")]
    public async Task SetAsync_StoresValueAndGetAsyncReturnsIt()
    {
        var key = "test_key";
        var value = "cached_value";

        await backend.SetAsync(key, value);
        var cached = await backend.GetAsync(key, typeof(string));

        Assert.Equal(value, cached);
    }

    [Fact(DisplayName = "SetAsync - Applies Absolute Expiration")]
    public async Task SetAsync_AppliesAbsoluteExpiration()
    {
        var key = "abs_exp_key";
        var value = "value";

        var options = new CachingBackendEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
        };

        await backend.SetAsync(key, value, options);

        // Wait a bit less than expiration time, should still exist
        await Task.Delay(3000);

        var cached = await backend.GetAsync(key, typeof(string));
        Assert.Equal(value, cached);

        // Wait for expiration
        await Task.Delay(3000);

        cached = await backend.GetAsync(key, typeof(string));
        Assert.Null(cached);
    }

    [Fact(DisplayName = "SetAsync - Applies Sliding Expiration")]
    public async Task SetAsync_AppliesSlidingExpiration()
    {
        var key = "sliding_exp_key";
        var value = "value";

        var options = new CachingBackendEntryOptions
        {
            SlidingExpiration = TimeSpan.FromSeconds(2)
        };

        await backend.SetAsync(key, value, options);

        // Access repeatedly before sliding expiration resets the timer
        for (int i = 0; i < 3; i++)
        {
            await Task.Delay(1000);
            var cached = await backend.GetAsync(key, typeof(string));
            Assert.Equal(value, cached);
        }

        // Wait longer than sliding expiration without access
        await Task.Delay(3000);
        var expired = await backend.GetAsync(key, typeof(string));
        Assert.Null(expired);
    }
}
