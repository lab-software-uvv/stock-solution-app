namespace StockSolution.Api.Features.Products;

public sealed class ProductsGroup : Group
{
    public ProductsGroup()
    {
        Configure("Products", ep => ep.Description(b => b.Produces(401)));
    }
}