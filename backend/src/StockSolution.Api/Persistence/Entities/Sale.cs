namespace StockSolution.Api.Persistence.Entities;

public class Sale : BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime SellingDate { get; set; }
    public decimal TotalValue { get; set; }
    public SaleStatusEnum Status { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public ICollection<SaleComercialProduct> ComercialProducts { get; set; }
    public ICollection<SaleProduct> Products { get; set; }

    public Sale(int userId, DateTime sellingDate, decimal totalValue, PaymentMethodEnum paymentMethod)
    {
        UserId = userId;
        SellingDate = sellingDate;
        TotalValue = totalValue;
        PaymentMethod = paymentMethod;
        Status = SaleStatusEnum.InElaboration;
    }
}
public enum SaleStatusEnum
{
    InElaboration,
    Finished,
    Canceled
}

public enum PaymentMethodEnum
{
    Debit,
    Credit,
    Money,
    Ticket
}