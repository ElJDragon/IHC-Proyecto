using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using GestionIncidentes.Application.Models;

namespace GestionIncidentes.Web.Services;

public class AdminTicketService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AdminTicketService> _logger;

    public AdminTicketService(HttpClient httpClient, ILogger<AdminTicketService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene las estadísticas del dashboard de administrador
    /// </summary>
    public async Task<AdminDashboardStatsDto?> GetDashboardStatsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/admin/tickets/stats/dashboard");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AdminDashboardStatsDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas del dashboard");
            return null;
        }
    }

    /// <summary>
    /// Lista todos los tickets con filtros opcionales
    /// </summary>
    public async Task<List<TicketResponseDto>> ListTicketsAsync(TicketFilterDto? filters = null)
    {
        try
        {
            var queryParams = new List<string>();
            
            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.Status))
                    queryParams.Add($"Status={Uri.EscapeDataString(filters.Status)}");
                
                if (!string.IsNullOrEmpty(filters.Priority))
                    queryParams.Add($"Priority={Uri.EscapeDataString(filters.Priority)}");
                
                if (!string.IsNullOrEmpty(filters.Location))
                    queryParams.Add($"Location={Uri.EscapeDataString(filters.Location)}");
                
                if (!string.IsNullOrEmpty(filters.Category))
                    queryParams.Add($"Category={Uri.EscapeDataString(filters.Category)}");
                
                if (filters.AssignedToUserId.HasValue)
                    queryParams.Add($"AssignedToUserId={filters.AssignedToUserId}");
                
                if (filters.CreatedByUserId.HasValue)
                    queryParams.Add($"CreatedByUserId={filters.CreatedByUserId}");
            }

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"/api/admin/tickets{queryString}");
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<List<TicketResponseDto>>() ?? new List<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar tickets");
            return new List<TicketResponseDto>();
        }
    }

    /// <summary>
    /// Obtiene un ticket específico por ID
    /// </summary>
    public async Task<TicketResponseDto?> GetTicketAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/admin/tickets/{id}");
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
    /// Crea un nuevo ticket
    /// </summary>
    public async Task<TicketResponseDto?> CreateTicketAsync(CreateTicketDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/admin/tickets", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TicketResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear ticket");
            return null;
        }
    }

    /// <summary>
    /// Actualiza un ticket existente
    /// </summary>
    public async Task<bool> UpdateTicketAsync(Guid id, UpdateTicketDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/admin/tickets/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar ticket {TicketId}", id);
            return false;
        }
    }

    /// <summary>
    /// Elimina un ticket
    /// </summary>
    public async Task<bool> DeleteTicketAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/admin/tickets/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar ticket {TicketId}", id);
            return false;
        }
    }

    /// <summary>
    /// Obtiene estadísticas generales de tickets
    /// </summary>
    public async Task<TicketStatsDto?> GetGeneralStatsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/admin/tickets/stats");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TicketStatsDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas generales");
            return null;
        }
    }
}
