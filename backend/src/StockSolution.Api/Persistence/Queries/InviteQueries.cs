using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Persistence.Queries;

public static class InviteQueries
{
    public static Task<Invite?> GetAvailableById(this DbSet<Invite> invites, Guid id)
        => invites.FirstOrDefaultAsync(x => x.Id == id && x.UserId == null);
}