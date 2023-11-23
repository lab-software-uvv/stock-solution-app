using Bogus;
using FastEndpoints;
using FastEndpoints.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockSolution.Api.Features.Auth;
using StockSolution.Api.Persistence;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace StockSolution.Api.Tests;

public class ApiFixture : TestFixture<Program>
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase("StockSolution")
        .WithUsername("postgres")
        .WithPassword("P4$$w0rd")
        .Build();
    
    public AppDbContext CreateDbContext() 
        => Services.GetRequiredService<AppDbContext>();
    
    public new Faker Fake => new();
    
    public ApiFixture(IMessageSink s) : base(s)
    {
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        // TODO: Open an issue in FastEndpoint's repo so we can discuss that the 
        // WebApplicationFactory should NOT be initialized in the constructor
        // but in the ConfigureApp method.
        _dbContainer.StartAsync().GetAwaiter().GetResult();
        
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }
        
        services.AddDatabase($"{_dbContainer.GetConnectionString()};Include Error Detail=true;");
    }

    protected override async Task SetupAsync()
    {
        var loginCommand = new LoginCommand(DataFactory.AdminLogin, DataFactory.AdminPassword);
        var (_, rsp) = await Client.POSTAsync<LoginEndpoint, LoginCommand, TokenResponse>(loginCommand);

        Client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {rsp.Token}");
    }

    protected override Task TearDownAsync() 
        => _dbContainer.StopAsync();
}