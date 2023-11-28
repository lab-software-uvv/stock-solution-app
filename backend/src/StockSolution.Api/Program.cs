using StockSolution.Api;
using FastEndpoints.Swagger;
using StockSolution.Api.Common.Exceptions;
using StockSolution.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddStockSolution(builder.Configuration);
builder.Services.SwaggerDocument();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("http://localhost:3000", "https://stock-solution-app.vercel.app") // Update with your React app's URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition")
            .AllowCredentials());
});



var app = builder.Build();
await app.InitAsync();

app.UseCors("CorsPolicy");
app.MapHub<NotificationsHub>("/notification-hub").RequireCors("CorsPolicy");

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseFastEndpoints(c => {
    c.Endpoints.RoutePrefix = "api";
});

app.UseSwaggerGen();

app.Run();