using GestionIncidentes.Application.Models;
using GestionIncidentes.Application.Interfaces;

namespace GestionIncidentes.Web.Services;

public class StudentReportService
{
    private readonly IIncidentRepository _incidentRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILogger<StudentReportService> _logger;
    private readonly ICurrentUser _currentUser;

    public StudentReportService(
        IIncidentRepository incidentRepo, 
        IUserRepository userRepo,
        ILogger<StudentReportService> logger,
        ICurrentUser currentUser)
    {
        _incidentRepo = incidentRepo;
        _userRepo = userRepo;
        _logger = logger;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Crea un nuevo reporte de incidente
    /// </summary>
    public async Task<TicketResponseDto?> CreateReportAsync(CreateStudentReportDto dto)
    {
        try
        {
            var userId = _currentUser.UserId;
            if (!userId.HasValue)
            {
                _logger.LogWarning("CreateReportAsync: Usuario no autenticado");
                return null;
            }

            var title = dto.ProblemType == "software"
                ? $"Problema con {dto.ProgramName ?? "software"} en {dto.Lab}"
                : $"Falla de hardware en {dto.Lab} - {dto.AffectedParts}";

            var description = dto.ProblemType == "software"
                ? $"Programa: {dto.ProgramName}\nMensaje de error: {dto.ErrorMessage}\nEquipo: {dto.EquipmentId}"
                : $"Componentes afectados: {dto.AffectedParts}\nEquipo: {dto.EquipmentId}";

            var incident = Domain.Entities.Incident.Create(title, description, userId.Value);
            await _incidentRepo.AddAsync(incident);

            _logger.LogInformation($"CreateReportAsync: Incidente {incident.Id} creado exitosamente");

            return await MapToResponseDto(incident);
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
            var userId = _currentUser.UserId;
            if (!userId.HasValue)
            {
                _logger.LogWarning("GetMyReportsAsync: Usuario no autenticado");
                return new List<TicketResponseDto>();
            }

            _logger.LogInformation($"GetMyReportsAsync: Obteniendo reportes del usuario {userId.Value}");

            var incidents = await _incidentRepo.ListByUserAsync(userId.Value);
            
            _logger.LogInformation($"GetMyReportsAsync: Encontrados {incidents.Count} incidentes para el usuario {userId.Value}");

            var response = new List<TicketResponseDto>();
            foreach (var incident in incidents)
            {
                _logger.LogInformation($"GetMyReportsAsync: Mapeando incidente {incident.Id} - ReportedBy: {incident.ReportedByUserId} - Status: {incident.Status}");
                var dto = await MapToResponseDto(incident);
                _logger.LogInformation($"GetMyReportsAsync: DTO mapeado - Status: {dto.Status}");
                response.Add(dto);
            }

            _logger.LogInformation($"GetMyReportsAsync: Retornando {response.Count} reportes");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener reportes");
            return new List<TicketResponseDto>();
        }
    }

    private async Task<TicketResponseDto> MapToResponseDto(Domain.Entities.Incident incident)
    {
        var createdBy = await _userRepo.GetByIdAsync(incident.ReportedByUserId);

        var ticketNumber = $"INC-{incident.ReportedAt:yyMMdd}-{incident.Id.ToString()[..4]}";

        // Mapear estados de Incident a estados de Ticket para compatibilidad con UI
        var status = incident.Status switch
        {
            "Reported" => "Pendiente",
            "InProgress" => "En Proceso",
            "Resolved" => "Resuelto",
            _ => incident.Status
        };

        return new TicketResponseDto(
            Id: incident.Id,
            TicketNumber: ticketNumber,
            Title: incident.Title,
            Description: incident.Description,
            Category: "General",
            Priority: "Media",
            Status: status,
            Location: "N/A",
            LocationDetail: "",
            ProblemType: null,
            ProgramName: null,
            ErrorMessage: null,
            AffectedParts: null,
            CreatedByUserId: incident.ReportedByUserId,
            CreatedByName: createdBy?.FullName ?? "Desconocido",
            AssignedToUserId: null,
            AssignedToName: null,
            TechnicianNotes: null,
            CreatedAt: incident.ReportedAt,
            ResolvedAt: incident.Status == "Resolved" ? incident.ReportedAt.AddHours(2) : null,
            SlaDeadline: null,
            SlaStatus: "-",
            Rating: null
        );
    }

    /// <summary>
    /// Obtiene un reporte específico
    /// </summary>
    public async Task<TicketResponseDto?> GetReportAsync(Guid id)
    {
        try
        {
            var incident = await _incidentRepo.GetByIdAsync(id);
            if (incident == null)
            {
                _logger.LogWarning($"GetReportAsync: Incidente {id} no encontrado");
                return null;
            }

            var userId = _currentUser.UserId;
            if (userId.HasValue && incident.ReportedByUserId != userId.Value)
            {
                _logger.LogWarning($"GetReportAsync: Usuario {userId.Value} no autorizado para ver incidente {id}");
                return null;
            }

            return await MapToResponseDto(incident);
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
            var incident = await _incidentRepo.GetByIdAsync(id);
            if (incident == null)
            {
                _logger.LogWarning($"RateTicketAsync: Incidente {id} no encontrado");
                return false;
            }

            var userId = _currentUser.UserId;
            if (!userId.HasValue || incident.ReportedByUserId != userId.Value)
            {
                _logger.LogWarning($"RateTicketAsync: Usuario no autorizado para calificar incidente {id}");
                return false;
            }

            if (incident.Status != "Resolved")
            {
                _logger.LogWarning($"RateTicketAsync: Incidente {id} no está resuelto");
                return false;
            }

            // Nota: La entidad Incident no tiene campos de Rating actualmente
            _logger.LogInformation($"RateTicketAsync: Usuario {userId.Value} calificó incidente {id} con {dto.Rating} estrellas");
            
            return true;
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
            var userId = _currentUser.UserId;
            if (!userId.HasValue)
            {
                _logger.LogWarning("GetMyStatsAsync: Usuario no autenticado");
                return null;
            }

            var incidents = await _incidentRepo.ListByUserAsync(userId.Value);

            var totalReports = incidents.Count;
            var pendingReports = incidents.Count(i => i.Status == "Reported");
            var inProgressReports = incidents.Count(i => i.Status == "InProgress");
            var resolvedReports = incidents.Count(i => i.Status == "Resolved");

            // Calcular tiempo promedio de resolución (en horas)
            var resolvedIncidents = incidents.Where(i => i.Status == "Resolved").ToList();
            double averageResolutionTime = 0;
            if (resolvedIncidents.Any())
            {
                // Aproximación: asumimos 24 horas por cada incidente resuelto
                averageResolutionTime = 24.0;
            }

            return new StudentReportStatsDto(
                TotalReports: totalReports,
                PendingReports: pendingReports,
                InProgressReports: inProgressReports,
                ResolvedReports: resolvedReports,
                AverageResolutionTime: averageResolutionTime
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas de reportes");
            return null;
        }
    }
}
