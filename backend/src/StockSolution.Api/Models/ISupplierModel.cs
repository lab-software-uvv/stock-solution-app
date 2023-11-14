
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Models;

public interface ISupplierModel
{
    string Code { get; set; }
    string TradingName { get; set; }
    string CNPJ { get; set; }
}