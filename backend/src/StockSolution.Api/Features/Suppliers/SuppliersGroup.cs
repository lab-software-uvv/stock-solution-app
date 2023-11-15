namespace StockSolution.Api.Features.Suppliers;

public sealed class SuppliersGroup : Group
{
    public SuppliersGroup()
    {
        Configure("Suppliers", ep => ep.Description(b => b.Produces(401)));
    }
}