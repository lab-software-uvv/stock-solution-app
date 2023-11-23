namespace StockSolution.Api.Features.Invites;

public sealed class InvitesGroup : Group
{
    public InvitesGroup()
    {
        Configure("Invites", ep => ep.Description(b => b.Produces(401)));
    }
}