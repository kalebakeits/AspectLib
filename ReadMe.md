# AspectLib

A **no-config** C# library of aspects using [MrAdvice](https://github.com/arxLabs/MrAdvice) to replace repetitive code with simple attributes.  
Built as a free alternative to existing AOP frameworks where tooling is often expensive or awkward to integrate into modern solutions.

This library simplifies cross-cutting concerns like caching without needing to wire up or manage infrastructure code manually. Designed to drop cleanly into .NET applications with minimal setup.

For more information [**read the docs**](https://github.com/kalebakeits/AspectLib/wiki)

## 📦 Installation

Install the package via NuGet:

```
dotnet add package AspectLib --version 1.0.0-pre-release-1
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

[Cache]
public static string ProcessData()
{
    // method body
}
```

When this method is called, the attribute logic will be invoked according to its implementation.

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

## 📅 Project Status

Active development. Breaking changes expected until 1.0.

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