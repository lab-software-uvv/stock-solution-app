using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace StockSolution.Api.Hubs;

public sealed class NotificationsHub : Hub<INotificationsHub>
{
    private readonly AppDbContext _context;
    private readonly IClock _clock;

    public NotificationsHub(AppDbContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public Task SendProductsNearExpiration(string daysAfterToday)
    {
        var days = int.Parse(daysAfterToday);
        var productsNearExpiration = _context.Products
            .AsNoTracking()
            .Where(p => p.ExpirationDate <= _clock.GetCurrentInstant().Plus(Duration.FromDays(days)))
            .ToList();

        
        return Clients.All.SendProductsNearExpiration(productsNearExpiration);
    }
}