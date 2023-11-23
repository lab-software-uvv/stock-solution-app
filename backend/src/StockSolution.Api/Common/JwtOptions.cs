namespace StockSolution.Api.Common;

/// <summary>
/// Implements the IOptions pattern for reading an 'JwtOptions' section from an <see cref="IConfiguration"/> object.
/// </summary>
public class JwtOptions
{
    internal const string Section = "Jwt";
    
    /// <summary>
    /// The signing key for the JWT.
    /// </summary>
    public required string SigningKey { get; set; }
}