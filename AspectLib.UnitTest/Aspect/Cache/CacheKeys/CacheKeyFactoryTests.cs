namespace AspectLib.UnitTest.Aspcect.Cache.CacheKeys;

using System.Collections.Generic;
using System.Reflection;
using AspectLib.Aspects.Caching.CacheKeys;

[Collection("Cache Key Tests")]
public class CacheKeyServiceTests
{
    private readonly CacheKeyService service = new();

    [Fact(DisplayName = "GenerateCacheKey - Replaces placeholders with argument values")]
    public void GenerateCacheKey_ReplacesPlaceholders()
    {
        var template = "Method_{arg1}_{arg2}";
        var args = new[]
        {
            new KeyValuePair<string, object>("arg1", 123),
            new KeyValuePair<string, object>("arg2", "abc")
        };

        var result = this.service.GenerateCacheKey(template, args);

        Assert.Equal("Method_123_abc", result);
    }

    [Fact(DisplayName = "GenerateCacheKey - Leaves unmatched placeholders intact")]
    public void GenerateCacheKey_LeavesUnmatchedPlaceholders()
    {
        var template = "Method_{arg1}_{arg3}";
        var args = new[]
        {
            new KeyValuePair<string, object>("arg1", 123),
            new KeyValuePair<string, object>("arg2", "abc")
        };

        var result = this.service.GenerateCacheKey(template, args);

        Assert.Equal("Method_123_{arg3}", result);
    }

    [Fact(DisplayName = "GenerateCacheKey - Generates template from MethodInfo and replaces args")]
    public void GenerateCacheKey_FromMethodInfo_ReplacesArgs()
    {
        MethodInfo methodInfo = typeof(DummyClass).GetMethod(nameof(DummyClass.SampleMethod))!;
        var args = new[]
        {
            new KeyValuePair<string, object>("param1", 42),
            new KeyValuePair<string, object>("param2", "test")
        };

        var result = this.service.GenerateCacheKey(methodInfo, args);

        Assert.Equal("SampleMethod_42_test", result);
    }

    private class DummyClass
    {
        public static void SampleMethod(int param1, string param2)
        {
            _ = $"{param1} + {param2}";
        }
    }
}
