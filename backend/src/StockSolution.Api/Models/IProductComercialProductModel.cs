
using StockSolution.Api.Persistence.Entities;
using System.Collections.ObjectModel;

namespace StockSolution.Api.Models;

public interface IProductComercialProductModel
{
    int ProductId { get; set; }
    Product Product { get; set; }

    int ComercialProductId { get; set; }
    ComercialProduct ComercialProduct { get; set; }

}