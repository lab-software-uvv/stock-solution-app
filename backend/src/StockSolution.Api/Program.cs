using StockSolution.Api;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStockSolution(builder.Configuration);
builder.Services.SwaggerDocument();

var app = builder.Build();
await app.InitAsync();

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
    app.UseSwaggerUi3();
}

app.Run();