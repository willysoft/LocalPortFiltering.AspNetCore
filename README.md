# LocalPortFiltering.AspNetCore

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

// Register services
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddLocalPortFiltering(options =>
{
    options.IncludeFailureMessage = true; // Enable failure message
});

var app = builder.Build();

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseWhen(context => context.Connection.LocalPort == 5099,
            appBuilder => appBuilder.UseHttpsRedirection());
app.UseAuthorization();
app.UseLocalPortFiltering();

// Configure endpoints
app.MapHealthChecks("/healthz").RequireLocalPortFiltering(allowPort: 5105);
app.MapControllers();

app.Run();
```

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
    [LocalPortFiltering(allowPort: 5105)]
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
app.MapHealthChecks("/healthz").RequireLocalPortFiltering(allowPort: 5105);
```

**Combine with Conditional Middleware**

You can use additional middleware conditionally based on the local port:

```csharp
app.UseWhen(context => context.Connection.LocalPort == 5099,
            appBuilder => appBuilder.UseHttpsRedirection());

```

## Contribution

Contributions are welcome! Feel free to submit issues or pull requests to improve the project.

## License

This project is licensed under the [MIT License](./LICENSE).