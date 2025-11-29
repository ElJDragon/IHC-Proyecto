using GestionIncidentes.Application.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GestionIncidentes.Web.Services;

public class BlazorCurrentUser : ICurrentUser
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BlazorCurrentUser(
        AuthenticationStateProvider authenticationStateProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? _user;

    private async Task<ClaimsPrincipal> GetUserAsync()
    {
        if (_user == null)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            _user = authState.User;
        }
        return _user;
    }

    public Guid? UserId
    {
        get
        {
            try
            {
                // Intentar obtener el usuario autenticado
                var user = GetUserAsync().GetAwaiter().GetResult();
                var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (Guid.TryParse(userIdClaim, out var guid))
                    return guid;

                // Si no hay usuario autenticado, usar el usuario de prueba por defecto
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
                var user = GetUserAsync().GetAwaiter().GetResult();
                var name = user?.FindFirst(ClaimTypes.Name)?.Value;
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
                var user = GetUserAsync().GetAwaiter().GetResult();
                var email = user?.FindFirst(ClaimTypes.Email)?.Value;
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
                var user = GetUserAsync().GetAwaiter().GetResult();
                // En desarrollo, siempre consideramos autenticado
                return user?.Identity?.IsAuthenticated ?? true;
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
                var user = GetUserAsync().GetAwaiter().GetResult();
                return user?.FindFirst("department_id")?.Value ?? "11111111-1111-1111-1111-111111111111";
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
                var user = GetUserAsync().GetAwaiter().GetResult();
                var claim = user?.FindFirst("role_level")?.Value;
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
