using System.Security.Claims;
using System.Text;
using GestionIncidentes.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        // Generar JWT
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()!),
        new Claim(ClaimTypes.Name, result.UserName!),
        new Claim(ClaimTypes.Role, result.Role!),
        new Claim(ClaimTypes.Email, Email)
    };

        var key = Encoding.ASCII.GetBytes("EstaClaveTieneExactamente32Bytes!!");
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // Redirigir a la página intermedia SetToken
        var redirectUrl = result.Role!.ToLower() switch
        {
            "admin" => "/admin/dashboard",
            "tecnico" => "/tecnico/dashboard",
            _ => "/usuario/dashboard"
        };

        var intermediatePageUrl = $"/SetToken?jwt={tokenString}&userId={result.UserId}&redirect={redirectUrl}";
        _logger.LogInformation($"[LoginPage] Redirigiendo a la página intermedia: {intermediatePageUrl}");

        return Redirect(intermediatePageUrl);
    }

}
