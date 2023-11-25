using StockSolution.Api.Models;
using System.Collections.ObjectModel;
using YamlDotNet.Core.Tokens;

namespace StockSolution.Api.Persistence.Entities
{
    public class ComercialProduct : BaseEntity, IComercialProductModel
    {
        public ComercialProduct() { }
        public ComercialProduct(string code, string name, string description, decimal price)
        {
            Code = code;
            Name = name;
            Description = description;
            Price = price;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<ProductComercialProduct> ProductComercialProduct { get; set; }

    }
}