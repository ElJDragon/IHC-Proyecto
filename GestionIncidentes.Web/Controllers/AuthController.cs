using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MediatR;
using GestionIncidentes.Application.Features.Auth.Commands;

namespace GestionIncidentes.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        _logger.LogInformation("=== [AuthController] Login POST - INICIO ===");
        _logger.LogInformation($"[AuthController] Login - Email recibido: {loginRequest.Email}");

        var command = new LoginCommand(loginRequest.Email, loginRequest.Password);
        _logger.LogInformation("[AuthController] Login - Enviando comando a MediatR");
        var result = await _mediator.Send(command);
        _logger.LogInformation($"[AuthController] Login - Resultado de MediatR: Success={result.Success}");

        if (!result.Success)
        {
            _logger.LogWarning($"[AuthController] Login FALLIDO - Email: {loginRequest.Email}, Mensaje: {result.Message}");
            return Unauthorized(new { message = result.Message });
        }

        _logger.LogInformation($"[AuthController] Login EXITOSO - UserId: {result.UserId}, UserName: {result.UserName}, Role: {result.Role}");

        // Crear claims para el usuario autenticado
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()!),
            new Claim(ClaimTypes.Name, result.UserName!),
            new Claim(ClaimTypes.Role, result.Role!),
            new Claim(ClaimTypes.Email, loginRequest.Email)
        };

        _logger.LogInformation($"[AuthController] Login - Claims creados: {claims.Count} claims");
        foreach (var claim in claims)
        {
            _logger.LogInformation($"[AuthController] Login - Claim: {claim.Type} = {claim.Value}");
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        _logger.LogInformation("[AuthController] Login - Creando cookie de autenticacion");
        // Crear cookie de autenticación
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            new AuthenticationProperties
            {
                IsPersistent = true, // Cookie persiste después de cerrar navegador
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

        _logger.LogInformation("[AuthController] Login - Cookie creada exitosamente, expira en 8 horas");
        _logger.LogInformation($"[AuthController] Login - URL de redireccion: {GetRedirectUrl(result.Role!)}");
        _logger.LogInformation("=== [AuthController] Login POST - FIN EXITOSO ===");

        return Ok(new
        {
            success = true,
            userId = result.UserId,
            userName = result.UserName,
            role = result.Role,
            redirectUrl = GetRedirectUrl(result.Role!)
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("=== [AuthController] Logout POST - INICIO ===");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.LogInformation("[AuthController] Logout - Cookie eliminada, usuario deslogueado");
        _logger.LogInformation("=== [AuthController] Logout POST - FIN ===");
        return Ok(new { success = true });
    }

    [HttpGet("check")]
    public IActionResult CheckAuth()
    {
        _logger.LogInformation("=== [AuthController] CheckAuth GET - INICIO ===");
        var isAuthenticated = User?.Identity?.IsAuthenticated ?? false;
        _logger.LogInformation($"[AuthController] CheckAuth - IsAuthenticated: {isAuthenticated}");
        
        if (!isAuthenticated)
        {
            _logger.LogInformation("[AuthController] CheckAuth - Usuario NO autenticado");
            _logger.LogInformation("=== [AuthController] CheckAuth GET - FIN ===");
            return Ok(new { authenticated = false });
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        _logger.LogInformation($"[AuthController] CheckAuth - UserId: {userId}, UserName: {userName}, Role: {role}, Email: {email}");
        _logger.LogInformation("=== [AuthController] CheckAuth GET - FIN ===");

        return Ok(new
        {
            authenticated = true,
            userId = userId,
            userName = userName,
            role = role,
            email = email
        });
    }

    private string GetRedirectUrl(string role)
    {
        return role.ToLower() switch
        {
            "admin" => "/admin/dashboard",
            "tecnico" => "/tecnico/dashboard",
            "usuario" => "/usuario/dashboard",
            _ => "/login"
        };
    }
}

public record LoginRequest(string Email, string Password);
