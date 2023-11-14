using StockSolution.Api.Models;

namespace StockSolution.Api.Persistence.Entities;

/// <summary>
/// This entity represents a employee associated to the organization.
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
