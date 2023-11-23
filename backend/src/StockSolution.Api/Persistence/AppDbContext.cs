using System.Reflection;
using EntityFramework.Exceptions.PostgreSQL;
using StockSolution.Api.Persistence.Entities;
using StockSolution.Api.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Persistence;

public class AppDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public required DbSet<User> Users { get; set; }
    public required DbSet<Invite> Invites { get; set; }
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}