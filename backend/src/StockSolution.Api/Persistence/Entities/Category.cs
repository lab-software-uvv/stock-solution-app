using StockSolution.Api.Models;

namespace StockSolution.Api.Persistence.Entities;

/// <summary>
/// This entity represents a category in which products are going to be associated with.
/// </summary>
public class Category : BaseEntity, ICategoryModel
{
    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string? Description { get; set; }
}
