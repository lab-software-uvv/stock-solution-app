using StockSolution.Api.Models;
using System.Collections.ObjectModel;

namespace StockSolution.Api.Persistence.Entities
{
    public class ProductComercialProduct : BaseEntity, IProductComercialProductModel
    {
        public ProductComercialProduct() { }
        public ProductComercialProduct(int productId, int comercialProductId)
        {
            ProductId = productId;
            ComercialProductId = comercialProductId;
        }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ComercialProductId { get; set; }
        public ComercialProduct ComercialProduct { get; set; }

    }
}