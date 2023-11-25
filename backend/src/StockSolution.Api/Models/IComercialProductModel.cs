
using StockSolution.Api.Persistence.Entities;
using System.Collections.ObjectModel;

namespace StockSolution.Api.Models;

public interface IComercialProductModel
{
    string Code { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    decimal Price { get; set; }
    ICollection<ProductComercialProduct> ProductComercialProduct { get; set; }
}