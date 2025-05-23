# LocalPortFiltering.AspNetCore

[![GitHub Actions](https://github.com/willysoft/LocalPortFiltering.AspNetCore/workflows/build-debug/badge.svg)](https://github.com/willysoft/LocalPortFiltering.AspNetCore/actions) [![GitHub Actions](https://github.com/willysoft/LocalPortFiltering.AspNetCore/workflows/build-release/badge.svg)](https://github.com/willysoft/LocalPortFiltering.AspNetCore/actions) [![Releases](https://img.shields.io/github/v/release/willysoft/LocalPortFiltering.AspNetCore.svg)](https://github.com/willysoft/LocalPortFiltering.AspNetCore/releases) [![NuGet](https://img.shields.io/nuget/vpre/LocalPortFiltering.AspNetCore.svg)](https://www.nuget.org/packages/LocalPortFiltering.AspNetCore/)

`LocalPortFiltering.AspNetCore` is a middleware and toolset for ASP.NET Core that enables filtering HTTP requests based on the local port. This package is ideal for scenarios where certain operations must be restricted to specific ports, enhancing application security and traffic control.

## Installation

Install the package via NuGet:

```bash
dotnet add package LocalPortFiltering.AspNetCore
```

## Getting Started

Here's an example of how to use `LocalPortFiltering` in an ASP.NET Core application:

### Setup in `Program.cs`

**Program.cs**

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add LocalPortFiltering service
builder.Services.AddLocalPortFiltering();

var app = builder.Build();

// Use LocalPortFiltering middleware
app.UseLocalPortFiltering();

// Define a GET endpoint with port filtering
app.MapGet("/service1", () => "Welcome to Service 1")
   .RequireLocalPortFiltering(allowPorts: 5105);

// Define another GET endpoint without port filtering
app.MapGet("/service2", () => "Welcome to Service 2");

app.Run();
```

## Why Use LocalPortFiltering.AspNetCore

While ASP.NET Core provides options like `RequireHost` to filter requests based on the `Host` header, it can be vulnerable to **host header spoofing** attacks. This can allow malicious actors to bypass security measures by falsifying the `Host` header.

`LocalPortFiltering.AspNetCore` enhances security by relying on the actual network connection's **local port** (`ConnectionInfo.LocalPort`) for filtering requests, making it immune to host header spoofing.

### Key Benefits:
- **Stronger security**: Prevents host header spoofing attacks by filtering based on the actual local port.
- **Port-based filtering**: Allows you to enforce restrictions on which ports are allowed for specific routes, such as health checks, ensuring that only trusted internal traffic can access sensitive endpoints.
- **Simple integration**: Easily integrates into your ASP.NET Core application, providing an extra layer of security for your health checks and other internal services.

For scenarios where you want to enforce stricter security and prevent potential attacks based on manipulated headers, `LocalPortFiltering.AspNetCore` is a highly recommended solution.

## Apply Filtering in Controllers

To restrict access to specific actions or controllers, use the `LocalPortFilteringAttribute`:

```csharp
using LocalPortFiltering.AspNetCore;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpGet]
    [LocalPortFiltering(allowPorts: 5105)]
    public IActionResult Get()
    {
        return Ok("This endpoint is only accessible on port 5105.");
    }
}
```

## Configuration Options

`LocalPortFilteringOptions` provides the following configuration:

- `IncludeFailureMessage`: Specifies whether to include a failure message in the response.
  - Default: `true`

You can configure it in `Program.cs`

```csharp
builder.Services.AddLocalPortFiltering(options =>
{
    options.IncludeFailureMessage = true;
});
```

## Advanced Features

Use `RequireLocalPortFiltering`
You can apply port-based filtering to specific endpoints:

```csharp
app.MapHealthChecks("/healthz").RequireLocalPortFiltering(allowPorts: 5105);
```

**Combine with Conditional Middleware**

You can use additional middleware conditionally based on the local port:

```csharp
app.UseWhen(context => context.Connection.LocalPort != 5105,
            appBuilder => appBuilder.UseHttpsRedirection());
```

## Contribution

Contributions are welcome! Feel free to submit issues or pull requests to improve the project.

## License

This project is licensed under the [MIT License](./LICENSE).
