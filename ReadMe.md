# AspectLib

A **no-config** C# library of aspects using [MrAdvice](https://github.com/arxLabs/MrAdvice) to replace repetitive code with simple attributes.  
Built as a free alternative to existing AOP frameworks where tooling is often expensive or awkward to integrate into modern solutions.

This library simplifies cross-cutting concerns like caching without needing to wire up or manage infrastructure code manually. Designed to drop cleanly into .NET applications with minimal setup.

## 📦 Installation

Install the package via NuGet:

```
dotnet add package AspectLib --version 1.0.0-pre-release
```

**Note:**  
This package includes [MrAdvice](https://github.com/arxLabs/MrAdvice) as a dependency and automatically enables method interception and build-time weaving when your project builds.

## 🚀 Usage

Once installed, you can apply the attribute to your methods like so:

```csharp
[Cache]
public async Task<int> ExampleMethod()
{
    // method body
}
```

## 📖 Example

```csharp
[Cache]
public static string ProcessData()
{
    // method body
}
```

When this method is called, the attribute logic will be invoked according to its implementation.

## 📖 Features

**Implemented:**
- **[Cache]** — Wraps a method call with a cache. If the cache is a hit, it is returned directly without invoking the methdo. Otherwise, the method is invoked and the result is stored in the cache.

**Planned:**
- Logging Aspect  
- Retry Aspect  
- Timeout Aspect  

## 📦 Requirements & Compatibility

- Requires .NET 8.0 (other versions untested)
- Method interception powered by MrAdvice (included automatically)

## 📅 Project Status

Active development. Breaking changes expected until 1.0.

## ✅ Roadmap

- Publish NuGet package  
- Add Logging, Retry, Timeout aspects  
- Set up CI/CD pipeline with build & test badge  
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