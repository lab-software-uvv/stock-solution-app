namespace StockSolution.Api.Persistence.Entities;

public class ProductComercialProduct : BaseEntity
{
        public ProductComercialProduct() { }
        public ProductComercialProduct(int productId, int comercialProductId, decimal quantity)
        {
            ProductId = productId;
            ComercialProductId = comercialProductId;
            Quantity = quantity;
        }

    public required int ComercialProductId { get; set; }
    public ComercialProduct? ComercialProduct { get; set; }

        public int ComercialProductId { get; set; }
        public ComercialProduct ComercialProduct { get; set; }
        public decimal Quantity { get; set; }

}