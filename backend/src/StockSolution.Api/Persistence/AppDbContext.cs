using System.Reflection;
using StockSolution.Api.Persistence.Entities;
using StockSolution.Api.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
    public required DbSet<EmployeeRole> EmployeesRoles { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Product> Products { get; set; }
    public required DbSet<Supplier> Suppliers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique(true);

        builder.Entity<Product>()
            .HasIndex(c => c.Code)
            .IsUnique(true);

        builder.Entity<Supplier>()
            .HasIndex(c => c.CNPJ)
            .IsUnique(true);

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}