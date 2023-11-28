namespace StockSolution.Api.Features.Auth;

public sealed class AuthGroup : Group
{
    public AuthGroup()
    {
        Configure("auth", ep => ep.Description(b => b.Produces(401)));
    }
}