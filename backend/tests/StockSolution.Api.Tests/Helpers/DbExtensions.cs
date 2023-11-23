using StockSolution.Api.Persistence;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Tests.Helpers;

public static class DbExtensions
{
    public static async Task<Guid> CreateInviteAsync(this AppDbContext context, string email)
    {
        var invite = new Invite
        {
            Id = Guid.NewGuid(),
            Email = email.ToLower(),
            RoleId = 1
        };

        await context.Invites.AddAsync(invite);
        await context.SaveChangesAsync();

        return invite.Id;
    }
}