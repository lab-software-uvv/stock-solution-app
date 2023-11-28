namespace StockSolution.Api.Persistence.Entities;

public class SaleProduct : BaseEntity
{
    public int SaleId { get; set; }
    public virtual Sale Sale { get; set; }
    
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal Value { get; set; }

    public SaleProduct(int saleId, int productId, decimal quantity, decimal value)
    {
        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        Value = value;
    }
}