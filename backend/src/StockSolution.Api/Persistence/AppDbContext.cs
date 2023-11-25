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
    public required DbSet<ComercialProduct> ComercialProducts { get; set; }
    public required DbSet<ProductComercialProduct> ProductComercialProducts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        #region Categoria

        builder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique(true);

        #endregion

        #region Produto

        builder.Entity<Product>()
            .HasIndex(c => c.Code)
            .IsUnique(true);

        builder.Entity<Product>()
            .Property(e => e.AquisitionDate)
            .HasColumnType("date");

        builder.Entity<Product>()
            .Property(e => e.ExpirationDate)
            .HasColumnType("date");

        #endregion

        #region Fornecedor

        builder.Entity<Supplier>()
            .HasIndex(c => c.CNPJ)
            .IsUnique(true)
            ;

        builder.Entity<Supplier>()
            .HasIndex(c => c.Code)
            .IsUnique(true)
            ;

        #endregion

        #region Produto Comercial

        builder.Entity<ComercialProduct>()
            .HasIndex(c => c.Code)
            .IsUnique(true)
            ;

        #endregion

        #region Produto Comercial Produto

        builder.Entity<ProductComercialProduct>()
            .HasKey(sc => new { sc.ProductId, sc.ComercialProductId });

        builder.Entity<ProductComercialProduct>()
            .HasOne(sc => sc.Product)
            .WithMany(s => s.ProductComercialProduct)
            .HasForeignKey(sc => sc.ProductId);

        builder.Entity<ProductComercialProduct>()
           .HasOne(sc => sc.ComercialProduct)
           .WithMany(s => s.ProductComercialProduct)
           .HasForeignKey(sc => sc.ComercialProductId);

        #endregion

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}