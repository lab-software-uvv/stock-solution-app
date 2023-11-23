using System.Security.Claims;
using CommunityToolkit.Diagnostics;
using FastEndpoints.Security;
using Microsoft.Extensions.Options;
using StockSolution.Api.Common;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Services;

[RegisterSingleton]
public class JwtGenerator
{
    private readonly JwtOptions _options;

    public JwtGenerator(IOptions<JwtOptions> options)
        => _options = options.Value;

    public string GenerateToken(User user, Role role)
    {
        Guard.IsNotNull(user);
        Guard.IsNotNull(role);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        return JWTBearer.CreateToken(_options.SigningKey,
            expireAt: DateTime.Now.AddDays(7),
            privileges: u =>
            {
                u.Roles.Add(role.Name);
                u.Claims.AddRange(claims);
            });
    }
}