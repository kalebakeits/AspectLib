namespace AspectLib.Aspects.Caching.CacheKeys;

using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// Default implementation of the <see cref="ICacheKeyFactory"/> interface.
/// </summary>
public partial class CacheKeyService : ICacheKeyFactory
{
    [GeneratedRegex("\\{(.+?)\\}")]
    private static partial Regex MyRegex();

    /// <inheritdoc/>
    public virtual string GenerateCacheKey(string template, KeyValuePair<string, object>[] args)
    {
        return MyRegex()
            .Replace(
                template,
                (m) =>
                {
                    var key = m.Groups[1].Value;
                    var arg = args.FirstOrDefault(a => a.Key == key);
                    return arg.Value?.ToString() ?? $"{{{key}}}";
                }
            );
    }

    /// <inheritdoc/>
    public virtual string GenerateCacheKey(
        MethodInfo methodInfo,
        params KeyValuePair<string, object>[] args
    )
    {
        var template =
            $"{methodInfo.Name}_{string.Join("_", methodInfo.GetParameters().Select(p => $"{{{p.Name}}}"))}";
        return GenerateCacheKey(template, args);
    }
}
