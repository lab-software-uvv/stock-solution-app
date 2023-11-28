using StockSolution.Api.Models;

namespace StockSolution.Api.Persistence.Entities;

/// <summary>
/// This entity represents a employee associated to the organization.
/// </summary>
public class Role : BaseEntity
{
    public required string Name { get; set; }
    
    public List<User> Users { get; set; } = null!;
}
