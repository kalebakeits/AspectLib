# AspectLib

[![Build and Test](https://github.com/kalebakeits/AspectLib/actions/workflows/test.yml/badge.svg)](https://github.com/kalebakeits/AspectLib/actions/workflows/test.yml)

A **no-config**(still customisable) C# library of aspects using [MrAdvice](https://github.com/ArxOne/MrAdvice) to replace repetitive code with simple attributes. Built as a free alternative to existing AOP frameworks where tooling is often expensive or awkward to integrate into modern solutions.
This library simplifies cross-cutting concerns like caching without needing to wire up or manage infrastructure code manually. Designed to drop cleanly into .NET applications with minimal setup.
For more information [**read the docs**](https://github.com/kalebakeits/AspectLib/wiki)

## 📦 Installation
Install the package via NuGet:

```
dotnet add package AspectLib --version 1.0.0
```

**Note:**  
This package includes [MrAdvice](https://github.com/ArxOne/MrAdvice) as a dependency and automatically enables method interception and build-time weaving when your project builds.

## 🚀 Usage
Once installed, you can apply the attribute to your methods like so:

```csharp   
[Cache]
public async Task<CachedObject> GetByIdAsync(int id)
{
    using var httpClient = new HttpClient();
    var response = await httpClient.GetAsync($"https://www.example.com/{id}");
    var content = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<CachedObject>(content);
}
```

**Without `[Cache]`** the equivalent code would be:

```csharp
public async Task<CachedObject> GetByIdAsync(int id)
{
    var cacheKey = $"GetByIdAsync_{id}";
    var cached = await this.cacheService.GetAsync<CachedObject>(cacheKey);
    if (cached != null)
        return cached;
    using var httpClient = new HttpClient();
    var response = await httpClient.GetAsync($"https://www.example.com/{id}");
    var content = await response.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<CachedObject>(content);
    await cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(30));
    return result;
}
```

## 📖 Features
**Implemented:**  
- [**[CacheAttribute]**](https://github.com/kalebakeits/AspectLib/wiki/CacheAttribute)

**Planned:**  
- Logging Aspect  
- Retry Aspect  
- Timeout Aspect  

## 📦 Requirements & Compatibility
- Requires .NET 8.0 (other versions untested)
- Method interception powered by MrAdvice (included automatically)

## ✅ Roadmap
- Add Logging, Retry, Timeout aspects  
- Write contribution guidelines & code style rules  

## 🤝 Contributing
Contributions are welcome.  
If you encounter a bug or have a feature request:
1. Open an issue describing the problem or enhancement.
2. Fork the repository.
3. Submit a pull request.
Fixes and improvements will be handled on a best-effort basis.

## 📜 License
MIT License. See LICENSE file for details.  
Use at your own risk.