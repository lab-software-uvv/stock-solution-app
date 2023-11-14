namespace StockSolution.Api.Features.Categories;

public sealed class CategoriesGroup : Group
{
    public CategoriesGroup()
    {
        Configure("Categories", ep => ep.Description(b => b.Produces(401)));
    }
}