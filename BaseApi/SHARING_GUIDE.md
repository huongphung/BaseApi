# BaseApi Project Sharing Guide

This guide explains how to share your BaseApi project as a common library for other projects to use.

## Overview

Your BaseApi project is structured with a `Core.Library` that contains reusable components. This can be shared in several ways:

1. **NuGet Package** (Recommended) - Professional distribution
2. **Git Submodule** - Direct source code sharing
3. **GitHub Template** - Repository template for new projects

## Option 1: NuGet Package Distribution

### Building the Package

```powershell
# Build package locally
.\build-package.ps1 -Version "1.0.0"

# Or manually
dotnet pack Core\BaseLibrary\BaseLibrary\Core\Core.Library\Core.Library.csproj -c Release -o nupkg
```

### Publishing to NuGet.org

1. **Create NuGet account**: https://www.nuget.org/
2. **Get API key** from your account
3. **Add to GitHub Secrets**: `NUGET_API_KEY`
4. **Push with tag**:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

### Publishing to GitHub Packages

The GitHub Actions workflow automatically publishes to GitHub Packages when you push a tag.

### Using the Package

```bash
# Add package source (if using GitHub Packages)
dotnet nuget add source https://nuget.pkg.github.com/yourusername/index.json -n github -u yourusername -p $GITHUB_TOKEN

# Install package
dotnet add package BaseApi.Core.Library
```

## Option 2: Git Submodule (Current Setup)

### For Other Projects

```bash
# Add submodule to existing project
git submodule add https://github.com/huongphung/BaseLibrary.git Core/BaseLibrary

# Initialize and update
git submodule update --init --recursive
```

### Updating Submodule

```bash
# In the main project
git submodule update --remote Core/BaseLibrary
git add Core/BaseLibrary
git commit -m "Update BaseLibrary submodule"
git push
```

## Option 3: GitHub Template Repository

1. **Go to repository settings**
2. **Enable "Template repository"**
3. **Others can use "Use this template"**

## Project Structure

```
BaseApi/
├── Core/
│   └── BaseLibrary/          # Shared library (submodule)
│       └── Core.Library/     # Main library project
├── Application/              # Application layer
├── Domain/                   # Domain layer
├── Infrastructure/           # Infrastructure layer
└── WebApi/                   # API layer
```

## Core.Library Features

### Services
- **Permission Management**: `DefinePermisionService`
- **Caching**: `CacheHelper`
- **HMAC Utilities**: `HMACUtil`

### Middleware
- **Tracing**: `TracingMiddleware`
- **Request/Response handling**

### Configuration
- **Swagger**: `ConfigureSwaggerOptions`
- **Dependency Injection**: `AddSwagger`, `AddEnvironment`, `AddCqrs`

### Models & Attributes
- **Permission Attributes**: `DefinePermisionAttribute`
- **Common Models**: `DefinePermisionModel`

## Usage Examples

### 1. Basic Setup

```csharp
// Program.cs
using BaseApi.Core.Library.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);

// Add core services
builder.Services.AddCoreLibrary();
builder.Services.AddSwagger();
builder.Services.AddEnvironment();
builder.Services.AddCqrs();

var app = builder.Build();

// Add middleware
app.UseTracingMiddleware();
app.UseSwagger();
app.UseSwaggerUI();
```

### 2. Permission System

```csharp
[DefinePermision("User", "Read")]
public class UserController : ControllerBase
{
    private readonly IDefinePermision _permissionService;
    
    public UserController(IDefinePermision permissionService)
    {
        _permissionService = permissionService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        // Permission is automatically checked
        return Ok();
    }
}
```

### 3. Caching

```csharp
public class UserService
{
    private readonly ICacheHelper _cache;
    
    public UserService(ICacheHelper cache)
    {
        _cache = cache;
    }
    
    public async Task<User> GetUserAsync(int id)
    {
        var cacheKey = $"user:{id}";
        return await _cache.GetOrSetAsync(cacheKey, 
            () => _userRepository.GetByIdAsync(id), 
            TimeSpan.FromMinutes(30));
    }
}
```

## Version Management

### Semantic Versioning

- **Major**: Breaking changes
- **Minor**: New features (backward compatible)
- **Patch**: Bug fixes (backward compatible)

### Release Process

1. **Update version** in `Core.Library.csproj`
2. **Create tag**: `git tag v1.0.0`
3. **Push tag**: `git push origin v1.0.0`
4. **GitHub Actions** automatically builds and publishes

## Contributing

### For Contributors

1. **Fork the repository**
2. **Create feature branch**: `git checkout -b feature/new-feature`
3. **Make changes**
4. **Test thoroughly**
5. **Submit pull request**

### For Maintainers

1. **Review pull requests**
2. **Run tests**: `dotnet test`
3. **Update version** if needed
4. **Create release tag**
5. **Monitor CI/CD pipeline**

## Troubleshooting

### Common Issues

1. **Submodule not updating**:
   ```bash
   git submodule update --init --recursive
   ```

2. **Package not found**:
   - Check package source configuration
   - Verify API keys for private feeds

3. **Build errors**:
   - Ensure .NET 8.0 SDK is installed
   - Check all dependencies are restored

### Support

- **Issues**: Create GitHub issue
- **Documentation**: Check README.md in Core.Library
- **Examples**: See WebApi project for usage examples

## Best Practices

1. **Keep Core.Library focused** on common functionality
2. **Use semantic versioning** for releases
3. **Document breaking changes** in release notes
4. **Test thoroughly** before publishing
5. **Maintain backward compatibility** when possible
6. **Use dependency injection** for services
7. **Follow SOLID principles** in library design
