namespace StockSolution.Api.Persistence.Entities;

public class ProductComercialProduct : BaseEntity
{
    public int ComercialProductId { get; set; }
    public ComercialProduct? ComercialProduct { get; set; }
    public decimal Quantity { get; set; }
}