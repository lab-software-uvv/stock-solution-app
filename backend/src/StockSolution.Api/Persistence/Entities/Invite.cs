namespace StockSolution.Api.Persistence.Entities;

/// <summary>
/// This entity representes an invite to a user to join the organization.
/// </summary>
public class Invite
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required int RoleId { get; set; }
    public int? UserId { get; set; }
    
    public Role? Role { get; set; }
    public User? User { get; set; }
}