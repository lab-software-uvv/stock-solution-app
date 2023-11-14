
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Models;

public interface IProductModel
{
    string Code { get; set; }
    string Name { get; set; }
    string? Description { get; set; }
    decimal Quantity { get; set; }
    Supplier Supplier { get; set; }
    decimal Price { get; set; }
    Category? Category { get; set; }
    DateTime AquisitionDate { get; set; }
    DateTime ExpirationDate { get; set; }
}