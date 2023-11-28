namespace StockSolution.Api.Features.ProductsComercialProduct;

public sealed class ProductsComercialProductGroup : Group
{
    public ProductsComercialProductGroup()
    {
        Configure("ProductsComercialProduct", ep => ep.Description(b => b.Produces(401)));
    }
}