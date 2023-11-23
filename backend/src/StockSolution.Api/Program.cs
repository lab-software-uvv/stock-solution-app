using StockSolution.Api;
using FastEndpoints.Swagger;
using Hellang.Middleware.ProblemDetails;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore();
builder.Services.AddStockSolution(builder.Configuration);
builder.Services.SwaggerDocument();

var app = builder.Build();
await app.InitAsync(app.Lifetime.ApplicationStarted).ConfigureAwait(false);

app.UseProblemDetails();

app.UseAuthentication();

app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Serializer.Options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
    app.UseSwaggerUi3();
}

app.Run();

public partial class Program { }