namespace StockSolution.Api.Persistence.Entities;

public class SaleComercialProduct : BaseEntity
{
    public required int SaleId { get; set; }
    public Sale? Sale { get; set; }

    public required int ComercialProductId { get; set; }
    public ComercialProduct? ComercialProduct { get; set; }

    public required decimal Quantity { get; set; }

    public required decimal Value { get; set; }
}