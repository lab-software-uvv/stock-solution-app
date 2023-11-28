namespace StockSolution.Api.Persistence.Entities;

public class SaleProduct : BaseEntity
{
    public required int SaleId { get; set; }
    public Sale? Sale { get; set; }
    
    public required int ProductId { get; set; }
    public Product? Product { get; set; }
    
    public required decimal Quantity { get; set; }
    
    public required decimal Value { get; set; }
}