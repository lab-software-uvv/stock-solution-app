namespace StockSolution.Api.Features.ComercialProducts;

public sealed class ComercialProductsGroup : Group
{
    public ComercialProductsGroup()
    {
        Configure("ComercialProducts", ep => ep.Description(b => b.Produces(401)));
    }
}