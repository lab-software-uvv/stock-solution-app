using NodaTime;

namespace StockSolution.Api.Persistence.Entities;

public class Sale : BaseEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }
    public Instant SellingDate { get; set; }
    public decimal TotalValue { get; set; }
    public SaleStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<SaleComercialProduct>? ComercialProducts { get; set; }
    public List<SaleProduct>? Products { get; set; }
}

public enum SaleStatus
{
    InElaboration,
    Finished,
    Canceled
}

public enum PaymentMethod
{
    Debit,
    Credit,
    Money,
    Ticket
}