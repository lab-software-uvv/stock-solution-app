using System.Reflection;
using StockSolution.Api.Persistence.Entities;
using StockSolution.Api.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using EntityFramework.Exceptions.PostgreSQL;

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
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<Invite> Invites { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Product> Products { get; set; }
    public required DbSet<Supplier> Suppliers { get; set; }
    public required DbSet<ComercialProduct> ComercialProducts { get; set; }
    public required DbSet<ProductComercialProduct> ProductComercialProducts { get; set; }
    public required DbSet<Sale> Sales { get; set; }
    
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
            .HasKey(sc => sc.Id);

        builder.Entity<ProductComercialProduct>()
            .HasOne(sc => sc.Product)
            .WithMany(s => s.ProductComercialProduct)
            .HasForeignKey(sc => sc.ProductId);

        builder.Entity<ProductComercialProduct>()
           .HasOne(sc => sc.ComercialProduct)
           .WithMany(s => s.ProductComercialProduct)
           .HasForeignKey(sc => sc.ComercialProductId);

        #endregion

        #region Venda

        builder.Entity<Sale>()
            .Property(e => e.SellingDate)
            .HasColumnType("date");

        #endregion

        #region Venda Produto Comercial

        builder.Entity<SaleComercialProduct>()
            .HasKey(scp => new { scp.SaleId, scp.ComercialProductId });
        
        builder.Entity<SaleComercialProduct>()
            .HasOne(sp => sp.Sale)
            .WithMany(s => s.ComercialProducts)
            .HasForeignKey(sp => sp.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SaleComercialProduct>()
            .HasOne(scp => scp.ComercialProduct)
            .WithMany(scp => scp.SaleComercialProducts)
            .HasForeignKey(sp => sp.ComercialProductId);

        #endregion
        
        #region Venda Produto

        builder.Entity<SaleProduct>()
            .HasKey(scp => new { scp.SaleId, scp.ProductId });
        
        builder.Entity<SaleProduct>()
            .HasOne(sp => sp.Sale)
            .WithMany(s => s.Products)
            .HasForeignKey(sp => sp.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SaleProduct>()
            .HasOne(scp => scp.Product)
            .WithMany(scp => scp.SaleProducts)
            .HasForeignKey(sp => sp.ProductId);

        #endregion

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}