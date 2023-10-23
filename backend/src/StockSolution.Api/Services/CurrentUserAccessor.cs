using System.Security.Claims;

namespace StockSolution.Api.Services;

[RegisterScoped]
public class CurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private string? RawUserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public int? UserId => RawUserId is not null && int.TryParse(RawUserId, out var userId) ? userId : null;
}