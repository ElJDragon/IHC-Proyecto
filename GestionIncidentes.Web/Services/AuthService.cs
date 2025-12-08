using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GestionIncidentes.Web.Services;

public class AuthService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        try
        {
            Console.WriteLine("[AuthService] IsAuthenticatedAsync - INICIANDO verificación");
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            Console.WriteLine("[AuthService] IsAuthenticatedAsync - Estado obtenido del provider");
            var user = authState.User;
            var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;
            Console.WriteLine($"[AuthService] IsAuthenticatedAsync - Resultado: {isAuthenticated}");
            if (isAuthenticated)
            {
                Console.WriteLine($"[AuthService] IsAuthenticatedAsync - Usuario: {user?.Identity?.Name}");
            }
            return isAuthenticated;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] IsAuthenticatedAsync - ERROR: {ex.Message}");
            Console.WriteLine($"[AuthService] IsAuthenticatedAsync - Stack: {ex.StackTrace}");
            return false;
        }
    }

    public async Task<Guid?> GetCurrentUserIdAsync()
    {
        try
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> GetCurrentUserNameAsync()
    {
        try
        {
            Console.WriteLine("[AuthService] GetCurrentUserNameAsync - Obteniendo nombre de usuario");
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userName = user?.FindFirst(ClaimTypes.Name)?.Value;
            Console.WriteLine($"[AuthService] GetCurrentUserNameAsync - UserName: {userName}");
            return userName;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] GetCurrentUserNameAsync - ERROR: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> GetCurrentUserRoleAsync()
    {
        try
        {
            Console.WriteLine("[AuthService] GetCurrentUserRoleAsync - Obteniendo rol de usuario");
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine($"[AuthService] GetCurrentUserRoleAsync - Role: {role}");
            return role;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] GetCurrentUserRoleAsync - ERROR: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> GetCurrentUserEmailAsync()
    {
        try
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user?.FindFirst(ClaimTypes.Email)?.Value;
        }
        catch
        {
            return null;
        }
    }
}
