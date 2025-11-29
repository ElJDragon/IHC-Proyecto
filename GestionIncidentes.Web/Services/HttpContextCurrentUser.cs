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

    public Guid? UserId
    {
        get
        {
            try
            {
                var userIdClaim = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(userIdClaim, out var guid))
                    return guid;
                
                // En desarrollo, retornar el usuario de prueba por defecto
                // Este es el usuario que creamos en la base de datos
                return Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
            catch
            {
                // En caso de error, retornar el usuario de prueba
                return Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
        }
    }

    public string? UserName
    {
        get
        {
            try
            {
                var name = User?.FindFirstValue(ClaimTypes.Name);
                return !string.IsNullOrEmpty(name) ? name : "Administrador";
            }
            catch
            {
                return "Administrador";
            }
        }
    }

    public string? Email
    {
        get
        {
            try
            {
                var email = User?.FindFirstValue(ClaimTypes.Email);
                return !string.IsNullOrEmpty(email) ? email : "admin@test.com";
            }
            catch
            {
                return "admin@test.com";
            }
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            try
            {
                // En desarrollo, siempre consideramos autenticado
                return User?.Identity?.IsAuthenticated ?? true;
            }
            catch
            {
                return true;
            }
        }
    }

    public string DepartmentId
    {
        get
        {
            try
            {
                var dept = User?.FindFirstValue("department_id");
                return !string.IsNullOrEmpty(dept) ? dept : "11111111-1111-1111-1111-111111111111";
            }
            catch
            {
                return "11111111-1111-1111-1111-111111111111";
            }
        }
    }

    public int? RoleLevel
    {
        get
        {
            try
            {
                var claim = User?.FindFirstValue("role_level");
                if (int.TryParse(claim, out int level))
                    return level;
                return 50; // Nivel DITIC por defecto
            }
            catch
            {
                return 50;
            }
        }
    }
}
