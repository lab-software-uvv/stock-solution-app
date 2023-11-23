using System.Reflection;
using CommunityToolkit.Diagnostics;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using StockSolution.Api.Common;
using StockSolution.Api.Common.Exceptions;
using ProblemDetailsExtensions = Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions;

namespace StockSolution.Api;

/// <summary>
/// Holding class for extension methods to <see cref="IServiceCollection"/> to add StockSolution's services and configurations.
/// </summary>
public static class StockSolutionDependencyInjectionExtensions
{
    /// <summary>
    /// Registers all of the StockSolution's infrastructure services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> to use</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained</returns>
    public static IServiceCollection AddStockSolution(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAsyncInitialization();
        services.AutoRegister();
        services.AddDatabase(configuration);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddHttpContextAccessor();
        services.AddSingleton<IClock>(SystemClock.Instance);

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Section));
        services.PostConfigure<JwtOptions>(o =>
        {
            Guard.IsNotNullOrWhiteSpace(o.SigningKey, "Jwt:SigningKey");
        });
        
        var jwtSigningKey = configuration["Jwt:SigningKey"];
        Guard.IsNotNullOrWhiteSpace(jwtSigningKey);
        
        services.AddJWTBearerAuth(jwtSigningKey);
        services.AddAuthorization();
        services.ConfigureProblemDetails();
        services.AddFastEndpoints(o =>
        {
            o.SourceGeneratorDiscoveredTypes.AddRange(Assembly.GetExecutingAssembly().DefinedTypes);
        });

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Guard.IsNotNullOrEmpty(connectionString);
        services.AddDatabase(connectionString);
    }

    public static void AddDatabase(this IServiceCollection services, string connectionString)
    {
        Guard.IsNotNullOrEmpty(connectionString);
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.UseNodaTime()));
    }

    private static void ConfigureProblemDetails(this IServiceCollection services)
    {
        ProblemDetailsExtensions.AddProblemDetails(services, opt =>
        {
            opt.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
            opt.MapToStatusCode<BadRequestException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
            opt.MapToStatusCode<ConflictException>(StatusCodes.Status409Conflict);
        });
    }
}