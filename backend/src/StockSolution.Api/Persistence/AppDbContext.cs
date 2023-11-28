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
    public required DbSet<Product> Products { get; set; }
    public required DbSet<Supplier> Suppliers { get; set; }
    public required DbSet<ComercialProduct> ComercialProducts { get; set; }
    public required DbSet<ProductComercialProduct> ProductComercialProducts { get; set; }
    public required DbSet<Sale> Sales { get; set; }
    
    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    //     
    //     builder.Entity<Product>()
    //         .HasIndex(c => c.Code)
    //         .IsUnique();
    //
    //     builder.Entity<Product>()
    //         .Property(e => e.AcquisitionDate)
    //         .HasColumnType("date");
    //
    //     builder.Entity<Product>()
    //         .Property(e => e.ExpirationDate)
    //         .HasColumnType("date");
    //
    //     builder.Entity<Supplier>()
    //         .HasIndex(c => c.CNPJ)
    //         .IsUnique();
    //
    //     builder.Entity<Supplier>()
    //         .HasIndex(c => c.Code)
    //         .IsUnique();
    //
    //     builder.Entity<ComercialProduct>()
    //         .HasIndex(c => c.Code)
    //         .IsUnique();
    //
    //     builder.Entity<ProductComercialProduct>()
    //         .HasKey(sc => new { sc.ProductId, sc.ComercialProductId });
    //
    //     builder.Entity<ProductComercialProduct>()
    //         .HasOne(sc => sc.Product)
    //         .WithMany(s => s.ProductComercialProduct)
    //         .HasForeignKey(sc => sc.ProductId);
    //
    //     builder.Entity<ProductComercialProduct>()
    //        .HasOne(sc => sc.ComercialProduct)
    //        .WithMany(s => s.ProductComercialProduct)
    //        .HasForeignKey(sc => sc.ComercialProductId);
    //
    //     base.OnModelCreating(builder);
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}