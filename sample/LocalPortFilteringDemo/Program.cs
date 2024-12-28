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
app.UseWhen(context => context.Connection.LocalPort != 5105,
            appBuilder => appBuilder.UseHttpsRedirection());
app.UseAuthorization();
app.UseLocalPortFiltering();

app.MapHealthChecks("/healthz").RequireLocalPortFiltering(allowPorts: 5105);
app.MapControllers();

app.Run();
