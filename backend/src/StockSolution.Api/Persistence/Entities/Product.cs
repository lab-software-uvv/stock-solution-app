using NodaTime;

namespace StockSolution.Api.Persistence.Entities;

/// <summary>
/// This entity represents a product associated to the organization.
/// </summary>
public class Product : BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Quantity { get; set; }
    public virtual Supplier Supplier { get; set; }
    public int SupplierId { get; set; }
    public decimal Price { get; set; }
    public Category? Category { get; set; }
    public int? CategoryId { get; set; }
    public DateTime AquisitionDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public ICollection<ProductComercialProduct> ProductComercialProduct { get; set; }
    public ICollection<SaleProduct> SaleProducts { get; set; }
}
