using System.Net.Http;
using System.Net.Http.Json;
using GestionIncidentes.Application.Models;

namespace GestionIncidentes.Web.Services;

public class StudentReportService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentReportService> _logger;

    public StudentReportService(HttpClient httpClient, ILogger<StudentReportService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Crea un nuevo reporte de incidente
    /// </summary>
    public async Task<TicketResponseDto?> CreateReportAsync(CreateStudentReportDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/student/reports", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear reporte");
            return null;
        }
    }

    /// <summary>
    /// Obtiene los reportes del estudiante actual
    /// </summary>
    public async Task<List<TicketResponseDto>> GetMyReportsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/student/reports/my-reports");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TicketResponseDto>>() ?? new List<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener reportes");
            return new List<TicketResponseDto>();
        }
    }

    /// <summary>
    /// Obtiene un reporte específico
    /// </summary>
    public async Task<TicketResponseDto?> GetReportAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/student/reports/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener reporte {ReportId}", id);
            return null;
        }
    }

    /// <summary>
    /// Califica un ticket resuelto
    /// </summary>
    public async Task<bool> RateTicketAsync(Guid id, RateTicketDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/student/reports/{id}/rate", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al calificar ticket {TicketId}", id);
            return false;
        }
    }

    /// <summary>
    /// Obtiene estadísticas de reportes del estudiante
    /// </summary>
    public async Task<StudentReportStatsDto?> GetMyStatsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/student/reports/stats");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StudentReportStatsDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas de reportes");
            return null;
        }
    }
}
