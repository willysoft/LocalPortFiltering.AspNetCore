var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddLocalPortFiltering();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseWhen(context => context.Connection.LocalPort == 5099,
            appBuilder => appBuilder.UseHttpsRedirection());
app.UseAuthorization();
app.UseLocalPortFiltering();

app.MapHealthChecks("/healthz").RequireLocalPortFiltering(allowPort: 5105);
app.MapControllers();

app.Run();
