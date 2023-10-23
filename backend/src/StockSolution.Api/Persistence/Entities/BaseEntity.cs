namespace StockSolution.Api.Persistence.Entities;

/// <summary>
/// This class represents the base entity for all entities in the application.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// The primary key for the entity.
    /// </summary>
    public int Id { get; set; }
}