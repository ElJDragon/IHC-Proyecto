using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GestionIncidentes.Web.Pages;

public class LogoutPageModel : PageModel
{
    private readonly ILogger<LogoutPageModel> _logger;

    public LogoutPageModel(ILogger<LogoutPageModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGet()
    {
        _logger.LogInformation("========================================");
        _logger.LogInformation("=== LogoutPage OnGet - INICIANDO ===");
        _logger.LogInformation("========================================");
        
        try
        {
            _logger.LogInformation("[LogoutPage] Usuario en contexto: {User}", HttpContext.User?.Identity?.Name ?? "No autenticado");
            _logger.LogInformation("[LogoutPage] IsAuthenticated: {IsAuth}", HttpContext.User?.Identity?.IsAuthenticated ?? false);
            
            _logger.LogInformation("[LogoutPage] Llamando a SignOutAsync...");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("[LogoutPage] SignOutAsync completado exitosamente");
            
            _logger.LogInformation("[LogoutPage] Cookie eliminada, sesion cerrada");
            _logger.LogInformation("[LogoutPage] Redirigiendo a LoginPage");
            
            return Redirect("/LoginPage");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[LogoutPage] ERROR durante el logout");
            _logger.LogError("[LogoutPage] Tipo: {Type}, Mensaje: {Message}", ex.GetType().Name, ex.Message);
            return Redirect("/LoginPage");
        }
        finally
        {
            _logger.LogInformation("========================================");
            _logger.LogInformation("=== LogoutPage OnGet - FINALIZADO ===");
            _logger.LogInformation("========================================");
        }
    }
}
