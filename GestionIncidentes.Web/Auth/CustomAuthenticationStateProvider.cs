using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using System.Security.Claims;

namespace GestionIncidentes.Web.Auth;

public class CustomAuthenticationStateProvider : ServerAuthenticationStateProvider
{
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;

    public CustomAuthenticationStateProvider(ILogger<CustomAuthenticationStateProvider> logger)
    {
        _logger = logger;
        _logger.LogInformation("[CustomAuthStateProvider] Constructor - Provider creado");
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogInformation("[CustomAuthStateProvider] GetAuthenticationStateAsync - INICIANDO");
        var state = await base.GetAuthenticationStateAsync();
        var isAuth = state.User?.Identity?.IsAuthenticated ?? false;
        var userName = state.User?.Identity?.Name;
        var role = state.User?.FindFirst(ClaimTypes.Role)?.Value;
        
        _logger.LogInformation($"[CustomAuthStateProvider] Estado obtenido - IsAuth: {isAuth}, User: {userName}, Role: {role}");
        return state;
    }

    public void NotifyAuthenticationStateChanged()
    {
        _logger.LogInformation("[CustomAuthStateProvider] NotifyAuthenticationStateChanged - NOTIFICANDO cambio de estado");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
