using StockSolution.Api;
using FastEndpoints.Swagger;
using StockSolution.Api.Common.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStockSolution(builder.Configuration);
builder.Services.SwaggerDocument();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition"));
});

var app = builder.Build();
await app.InitAsync();

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.UseCors("CorsPolicy");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
    app.UseSwaggerUi3();
}

app.Run();