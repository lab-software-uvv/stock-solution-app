namespace StockSolution.Api.Persistence.Entities;

public class SaleComercialProduct : BaseEntity
{
    public int SaleId { get; set; }
    public virtual Sale Sale { get; set; }
    
    public int ComercialProductId { get; set; }
    public virtual ComercialProduct ComercialProduct { get; set; }
    
    public decimal Quantity { get; set; }
    
    public decimal Value { get; set; }

    public SaleComercialProduct(int saleId, int comercialProductId, decimal quantity, decimal value)
    {
        SaleId = saleId;
        ComercialProductId = comercialProductId;
        Quantity = quantity;
        Value = value;
    }
}