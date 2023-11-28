using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace StockSolution.Api.Persistence.Entities;

public class User : BaseEntity
{
    public required string Name { get; set; }
    public required string Cpf { get; set; }
    public required LocalDate BirthDate { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required int RoleId { get; set; }
    public Role? Role { get; set; }
}