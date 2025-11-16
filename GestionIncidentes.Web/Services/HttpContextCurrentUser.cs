using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using GestionIncidentes.Application.Interfaces;

namespace GestionIncidentes.Infrastructure.Services;

public class HttpContextCurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string? UserId
    {
        get
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

    public string? Email
    {
        get
        {
            return User?.FindFirstValue(ClaimTypes.Email);
        }
    }

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public string DepartmentId
    {
        get
        {
            return User?.FindFirstValue("department_id") ?? string.Empty;
        }
    }

    public int? RoleLevel
    {
        get
        {
            var claim = User?.FindFirstValue("role_level");
            if (int.TryParse(claim, out int level))
                return level;
            return null;
        }
    }
}
