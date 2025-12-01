using System.Net.Http;
using System.Net.Http.Json;
using GestionIncidentes.Application.Models;

namespace GestionIncidentes.Web.Services;

public class TechnicianTicketService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TechnicianTicketService> _logger;

    public TechnicianTicketService(HttpClient httpClient, ILogger<TechnicianTicketService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene los tickets asignados al técnico actual
    /// </summary>
    public async Task<List<TicketResponseDto>> GetMyTicketsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/technician/tickets/my-tickets");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TicketResponseDto>>() ?? new List<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tickets asignados");
            return new List<TicketResponseDto>();
        }
    }

    /// <summary>
    /// Obtiene un ticket específico
    /// </summary>
    public async Task<TicketResponseDto?> GetTicketAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/technician/tickets/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ticket {TicketId}", id);
            return null;
        }
    }

    /// <summary>
    /// Cambia el estado de un ticket
    /// </summary>
    public async Task<bool> ChangeStatusAsync(Guid id, ChangeTicketStatusDto dto)
    {
        try
        {
            var response = await _httpClient.PatchAsJsonAsync($"/api/technician/tickets/{id}/status", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar estado del ticket {TicketId}", id);
            return false;
        }
    }

    /// <summary>
    /// Cierra un ticket con solución
    /// </summary>
    public async Task<bool> CloseTicketAsync(Guid id, CloseTicketDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/technician/tickets/{id}/close", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cerrar ticket {TicketId}", id);
            return false;
        }
    }

    /// <summary>
    /// Actualiza las notas del técnico
    /// </summary>
    public async Task<bool> UpdateNotesAsync(Guid id, string notes)
    {
        try
        {
            var dto = new UpdateTechnicianNotesDto { TechnicianNotes = notes };
            var response = await _httpClient.PatchAsJsonAsync($"/api/technician/tickets/{id}/notes", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar notas del ticket {TicketId}", id);
            return false;
        }
    }

    /// <summary>
    /// Obtiene tickets filtrados por estado
    /// </summary>
    public async Task<List<TicketResponseDto>> GetTicketsByStatusAsync(string status)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/technician/tickets?status={Uri.EscapeDataString(status)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TicketResponseDto>>() ?? new List<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tickets por estado");
            return new List<TicketResponseDto>();
        }
    }

    /// <summary>
    /// Obtiene estadísticas personales del técnico
    /// </summary>
    public async Task<TechnicianStatsDto?> GetMyStatsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/technician/tickets/stats/my-stats");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TechnicianStatsDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas personales");
            return null;
        }
    }

    /// <summary>
    /// Obtiene tickets con violación de SLA
    /// </summary>
    public async Task<List<TicketResponseDto>> GetTicketsWithSlaViolationAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/technician/tickets/sla-violations");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TicketResponseDto>>() ?? new List<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tickets con violación de SLA");
            return new List<TicketResponseDto>();
        }
    }
}
