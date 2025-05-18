namespace AspectLib.Aspects.Caching;

using System;
using System.Reflection;
using System.Threading.Tasks;
using ArxOne.MrAdvice.Advice;
using AspectLib.Aspects.Caching.CacheKeys;
using AspectLib.Aspects.Caching.CachingBackend;
using AspectLib.ServiceResolver;

/// <summary>
/// Caches the result of the method using the configured caching backend.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class CacheAttribute : Attribute, IMethodAsyncAdvice
{
    private readonly string? cacheKeyTemplate;

    private readonly CachingBackendEntryOptions Options;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheAttribute"/> class.
    /// </summary>
    /// <param name="cacheKeyTemplate"></param>
    /// <param name="absoluteExpirationSeconds"></param>
    /// <param name="slidingExpirationSeconds"></param>
    /// <param name="priority"></param>
    /// <param name="region"></param>
    /// <param name="enableCompression"></param>
    /// <param name="compressionThresholdBytes"></param>
    /// <param name="cacheNullValues"></param>
    /// <param name="enableBackgroundRefresh"></param>
    /// <param name="backgroundRefreshThresholdPercent"></param>
    /// <param name="enableMetrics"></param>
    /// <param name="serializerName"></param>
    public CacheAttribute(
        string? cacheKeyTemplate = null,
        int absoluteExpirationSeconds = -1,
        int slidingExpirationSeconds = -1,
        CachePriority priority = CachePriority.Normal,
        string? region = null,
        bool enableCompression = false,
        int compressionThresholdBytes = 10240,
        bool cacheNullValues = false,
        bool enableBackgroundRefresh = false,
        int backgroundRefreshThresholdPercent = 80,
        bool enableMetrics = false,
        string? serializerName = null
    )
    {
        this.cacheKeyTemplate = cacheKeyTemplate;

        this.Options = new CachingBackendEntryOptions
        {
            AbsoluteExpirationRelativeToNow =
                absoluteExpirationSeconds > 0
                    ? TimeSpan.FromSeconds(absoluteExpirationSeconds)
                    : null,
            SlidingExpiration =
                slidingExpirationSeconds > 0
                    ? TimeSpan.FromSeconds(slidingExpirationSeconds)
                    : null,
            Priority = priority,
            Region = region,
            EnableCompression = enableCompression,
            CompressionThresholdBytes = compressionThresholdBytes,
            CacheNullValues = cacheNullValues,
            EnableBackgroundRefresh = enableBackgroundRefresh,
            BackgroundRefreshThresholdPercent = backgroundRefreshThresholdPercent,
            EnableMetrics = enableMetrics,
            SerializerName = serializerName
        };
    }

    /// <summary>
    /// Caches the result of the method using the configured caching backend.
    /// </summary>
    /// <param name="context">The method advice context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Advise(MethodAsyncAdviceContext context)
    {
        ICachingBackend backend = AspectServiceResolver.Resolve<ICachingBackend>();
        ICacheKeyFactory cacheKeyFactory = AspectServiceResolver.Resolve<ICacheKeyFactory>();

        MethodInfo methodInfo = (MethodInfo)context.TargetMethod;

        ParameterInfo[] parameters = methodInfo.GetParameters();

        KeyValuePair<string, object>[] args = parameters
            .Select(
                (p, i) =>
                    new KeyValuePair<string, object>(p.Name ?? $"arg{i}", context.Arguments[i])
            )
            .ToArray();

        string cacheKey =
            this.cacheKeyTemplate == null
                ? cacheKeyFactory.GenerateCacheKey(methodInfo, args)
                : cacheKeyFactory.GenerateCacheKey(this.cacheKeyTemplate, args);

        object? result = await backend.GetAsync(cacheKey, GetReturnType(methodInfo));

        if (result == null)
        {
            result = await GetMethodResult(context, methodInfo);
            await backend.SetAsync(cacheKey, result, this.Options);
            return;
        }

        SetContextReturnValue(context, methodInfo, result);
    }

    private static async Task<object?> GetMethodResult(
        MethodAsyncAdviceContext context,
        MethodInfo methodInfo
    )
    {
        await context.ProceedAsync();

        Type returnType = methodInfo.ReturnType;

        if (!IsTargetMethodAsync(returnType))
        {
            return context.ReturnValue;
        }

        if (returnType.IsGenericType && context.ReturnValue != null)
        {
            var genericDef = returnType.GetGenericTypeDefinition();

            if (genericDef == typeof(Task<>) || genericDef == typeof(ValueTask<>))
            {
                try
                {
                    dynamic awaitable = context.ReturnValue;
                    return await awaitable;
                }
                catch
                {
                    return null;
                }
            }
        }

        return null;
    }

    private static void SetContextReturnValue(
        MethodAsyncAdviceContext context,
        MethodInfo methodInfo,
        object? value
    )
    {
        if (!IsTargetMethodAsync(methodInfo.ReturnType))
        {
            context.ReturnValue = value;
            return;
        }

        if (methodInfo.ReturnType.IsGenericType && value != null)
        {
            var genericDef = methodInfo.ReturnType.GetGenericTypeDefinition();
            var genericArg = methodInfo.ReturnType.GetGenericArguments()[0];

            if (genericDef == typeof(Task<>))
            {
                context.ReturnValue = Task.FromResult(value);
            }
            else if (genericDef == typeof(ValueTask<>))
            {
                // Create ValueTask<T> instance using Activator
                var valueTaskType = typeof(ValueTask<>).MakeGenericType(genericArg);
                context.ReturnValue = Activator.CreateInstance(valueTaskType, value);
            }
        }
    }

    /// <summary>
    /// Checks if the method is asynchronous.
    /// </summary>
    /// <param name="returnType">The return type of the method.</param>
    /// <returns><c>true</c> if the method is asynchronous, <c>false</c> otherwise.</returns>
    private static bool IsTargetMethodAsync(Type returnType)
    {
        if (returnType == null)
            return false;

        var fullName = returnType.FullName;

        if (
            fullName == "System.Threading.Tasks.Task"
            || fullName == "System.Threading.Tasks.ValueTask"
        )
            return true;

        if (returnType.IsGenericType)
        {
            var genericDefName = returnType.GetGenericTypeDefinition().FullName;
            if (
                genericDefName == "System.Threading.Tasks.Task`1"
                || genericDefName == "System.Threading.Tasks.ValueTask`1"
            )
                return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the return type of the method or the generic argument if the method is asynchronous.
    /// </summary>
    /// <param name="methodInfo">The method information.</param>
    /// <returns>The return type of the method.</returns>
    private static Type GetReturnType(MethodInfo methodInfo)
    {
        if (IsTargetMethodAsync(methodInfo.ReturnType))
            return methodInfo.ReturnType.GetGenericArguments()[0];
        return methodInfo.ReturnType;
    }
}
