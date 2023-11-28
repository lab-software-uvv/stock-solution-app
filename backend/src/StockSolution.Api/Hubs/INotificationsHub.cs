using SignalRSwaggerGen.Attributes;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Hubs;

[SignalRHub(path: "/notification-hub")]
public interface INotificationsHub
{
    [SignalRMethod("SendProductsNearExpiration")]
    Task SendProductsNearExpiration(List<Product> products);
}