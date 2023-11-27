using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Hubs;

public sealed class NotificationsHub : Hub<INotificationsHub>
{
    private readonly AppDbContext _context;
    public NotificationsHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task SendProductsNearExpiration(string daysAfterToday)
    {
        var productsNearExpiration = _context.Products
            .AsNoTracking()
            .Where(p => p.ExpirationDate <= DateTime.Now.AddDays(int.Parse(daysAfterToday)))
            .ToList();

        
        await Clients.All.SendProductsNearExpiration(productsNearExpiration);

    }
}