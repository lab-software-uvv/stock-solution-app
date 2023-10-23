using NodaTime;

namespace StockSolution.Api.Persistence.Entities;

/// <summary>
/// This interface is used to mark entities that are auditable.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// The timestamp when the entity was created.
    /// </summary>
    public Instant CreatedAt { get; set; }

    /// <summary>
    /// The timestamp when the entity was last updated.
    /// </summary>
    public Instant UpdatedAt { get; set; }

    /// <summary>
    /// The identifier of the user who created the entity. It can be null if the entity has not been created by any user (e.g., system generated).
    /// </summary>
    public int? CreatedBy { get; set; }

    /// <summary>
    /// The identifier of the user who last updated the entity. It can be null if the entity has not been updated by any user (e.g., system generated).
    /// </summary>
    public int? UpdatedBy { get; set; }
}

