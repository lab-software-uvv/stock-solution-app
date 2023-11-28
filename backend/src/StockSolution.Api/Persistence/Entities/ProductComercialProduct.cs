namespace StockSolution.Api.Persistence.Entities;

public class ProductComercialProduct : BaseEntity
{
    public required int ProductId { get; set; }
    public Product? Product { get; set; }

    public required int ComercialProductId { get; set; }
    public ComercialProduct? ComercialProduct { get; set; }

}