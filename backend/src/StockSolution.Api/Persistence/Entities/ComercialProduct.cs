namespace StockSolution.Api.Persistence.Entities;

public class ComercialProduct : BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    
    public List<ProductComercialProduct>? ProductComercialProduct { get; set; }
    public List<SaleComercialProduct>? SaleComercialProducts { get; set; }
}