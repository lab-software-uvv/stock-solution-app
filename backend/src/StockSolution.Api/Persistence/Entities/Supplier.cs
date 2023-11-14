using StockSolution.Api.Models;

namespace StockSolution.Api.Persistence.Entities
{
    public class Supplier: BaseEntity, ISupplierModel
    {
        public Supplier() { }
        public Supplier(string code, string tradingName, string cnpj)
        {
            Code = code;
            TradingName = tradingName;
            CNPJ = cnpj;
        }

        public string Code { get; set; }
        public string TradingName { get; set; }
        public string CNPJ { get; set; }

    }
}