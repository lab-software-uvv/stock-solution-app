using StockSolution.Api.Models;

namespace StockSolution.Api.Persistence.Entities;

/// <summary>
/// This entity represents a employee associated to the organization.
/// </summary>
public class EmployeeRole : BaseEntity, IEmployeeRoleModel
{
    public required string Description { get; set; }
    public List<User> Users { get; set; } = null!;
}
