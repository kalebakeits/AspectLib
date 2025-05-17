# AspectLib

A **no-config** C# library of aspects using [MrAdvice](https://github.com/arxLabs/MrAdvice) with the goal of replacing repetitive code with simple attributes.
Built as a free alternative to existing AOP frameworks where existing tooling is quite expensive or not a easy to integrate into solutions.


## 📦 Installation

To use this package:

1. Clone this repository or add it via a local project reference.
2. Install the required dependency [MrAdvice](https://github.com/arxLabs/MrAdvice):

```
dotnet add package MrAdvice
```

This package depends on MrAdvice for method interception and build-time weaving functionality.

## 🚀 Usage

Once installed, you can apply the attribute to your methods like so:

```csharp
[Cache]
public async Task<int> ExampleMethod()
{
    // method body
}
```

**Note:**  
Ensure your project is properly wired to use MrAdvice. Refer to [MrAdvice documentation](https://github.com/arxLabs/MrAdvice) for setup instructions.

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
- **[Cache]** — wraps the call to a method with a cache.

**Planned:**
- Logging Aspect  
- Retry Aspect  
- Timeout Aspect  

## 📦 Requirements & Compatibility

- Requires [MrAdvice](https://github.com/arxLabs/MrAdvice) for method interception.
- Tested on .NET 8.0. Other versions untested.

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