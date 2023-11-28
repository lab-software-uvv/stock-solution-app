using StockSolution.Api.Models;

namespace StockSolution.Api.Persistence.Entities;

public class Supplier: BaseEntity, ISupplierModel
{
    public required string Code { get; set; }
    public required string TradingName { get; set; }
    public required string CNPJ { get; set; }

}