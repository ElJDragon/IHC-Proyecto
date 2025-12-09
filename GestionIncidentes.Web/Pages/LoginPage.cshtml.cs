using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MediatR;
using GestionIncidentes.Application.Features.Auth.Commands;

namespace GestionIncidentes.Web.Pages;

[IgnoreAntiforgeryToken]
public class LoginPageModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoginPageModel> _logger;

    public LoginPageModel(IMediator mediator, ILogger<LoginPageModel> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [BindProperty]
    public string Email { get; set; } = "";
    
    [BindProperty]
    public string Password { get; set; } = "";
    
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        _logger.LogInformation("=== LoginPage OnGet - Mostrando formulario ===");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation($"=== LoginPage OnPost - Email: {Email} ===");

        var command = new LoginCommand(Email, Password);
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning($"[LoginPage] Login fallido para: {Email}");
            ErrorMessage = result.Message;
            return Page();
        }

        _logger.LogInformation($"[LoginPage] Login exitoso - UserId: {result.UserId}, Role: {result.Role}");

        // Crear claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()!),
            new Claim(ClaimTypes.Name, result.UserName!),
            new Claim(ClaimTypes.Role, result.Role!),
            new Claim(ClaimTypes.Email, Email)
        };

        _logger.LogInformation($"[LoginPage] Claims creados: {claims.Count}");

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        _logger.LogInformation("[LoginPage] Creando cookie de autenticacion");
        
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });

        _logger.LogInformation("[LoginPage] Cookie creada exitosamente");

        var redirectUrl = result.Role!.ToLower() switch
        {
            "admin" => "/admin/dashboard",
            "tecnico" => "/tecnico/dashboard",
            _ => "/usuario/dashboard"
        };

        _logger.LogInformation($"[LoginPage] Redirigiendo a: {redirectUrl}");
        
        return Redirect(redirectUrl);
    }
}
