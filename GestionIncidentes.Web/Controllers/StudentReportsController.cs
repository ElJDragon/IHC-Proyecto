using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Models;
using GestionIncidentes.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionIncidentes.Web.Controllers;

[ApiController]
[Route("api/student/reports")]
[Authorize] // Simplificado - cualquier usuario autenticado
public class StudentReportsController : ControllerBase
{
    private readonly IIncidentRepository _incidentRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILogger<StudentReportsController> _logger;

    public StudentReportsController(
        IIncidentRepository incidentRepo, 
        IUserRepository userRepo,
        ILogger<StudentReportsController> logger)
    {
        _incidentRepo = incidentRepo;
        _userRepo = userRepo;
        _logger = logger;
    }

    // ==================== Crear Reporte (Wizard Form) ====================
    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] CreateStudentReportDto dto)
    {
        var studentId = GetCurrentUserId();
        if (studentId == Guid.Empty)
        {
            _logger.LogWarning("CreateReport: Usuario no autenticado");
            return Unauthorized();
        }

        _logger.LogInformation($"CreateReport: Usuario {studentId} creando reporte en {dto.Lab}");

        // Generar título automático basado en el tipo de problema
        var title = dto.ProblemType == "software"
            ? $"Problema con {dto.ProgramName ?? "software"} en {dto.Lab}"
            : $"Falla de hardware en {dto.Lab} - {dto.AffectedParts}";

        // Generar descripción automática
        var description = dto.ProblemType == "software"
            ? $"Programa: {dto.ProgramName}\nMensaje de error: {dto.ErrorMessage}\nEquipo: {dto.EquipmentId}"
            : $"Componentes afectados: {dto.AffectedParts}\nEquipo: {dto.EquipmentId}";

        // Crear el incidente usando el método estático de la entidad
        var incident = Incident.Create(title, description, studentId);

        await _incidentRepo.AddAsync(incident);

        _logger.LogInformation($"CreateReport: Incidente {incident.Id} creado exitosamente");

        return CreatedAtAction(nameof(GetReport), new { id = incident.Id }, await MapToResponseDto(incident));
    }

    // ==================== Listar Mis Reportes ====================
    [HttpGet("my-reports")]
    public async Task<IActionResult> GetMyReports()
    {
        var studentId = GetCurrentUserId();
        if (studentId == Guid.Empty)
        {
            _logger.LogWarning("GetMyReports: Usuario no autenticado");
            return Unauthorized();
        }

        _logger.LogInformation($"GetMyReports: Obteniendo reportes del usuario {studentId}");

        var incidents = await _incidentRepo.ListByUserAsync(studentId);
        
        _logger.LogInformation($"GetMyReports: Encontrados {incidents.Count} incidentes");

        var response = new List<TicketResponseDto>();
        foreach (var incident in incidents)
        {
            response.Add(await MapToResponseDto(incident));
        }

        return Ok(response);
    }

    // ==================== Obtener Reporte por ID ====================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReport(Guid id)
    {
        var incident = await _incidentRepo.GetByIdAsync(id);
        if (incident == null)
            return NotFound(new { message = "Reporte no encontrado" });

        var studentId = GetCurrentUserId();
        if (incident.ReportedByUserId != studentId)
            return Forbid();

        return Ok(await MapToResponseDto(incident));
    }

    // ==================== Valorar Ticket Resuelto ====================
    [HttpPost("{id}/rate")]
    public async Task<IActionResult> RateTicket(Guid id, [FromBody] RateTicketDto dto)
    {
        var incident = await _incidentRepo.GetByIdAsync(id);
        if (incident == null)
            return NotFound(new { message = "Reporte no encontrado" });

        var studentId = GetCurrentUserId();
        if (incident.ReportedByUserId != studentId)
            return Forbid();

        if (incident.Status != "Resolved")
            return BadRequest(new { message = "Solo puedes valorar incidentes resueltos" });

        // Nota: La entidad Incident no tiene Rating/FeedbackComment actualmente
        // Por ahora solo retornamos OK
        _logger.LogInformation($"RateTicket: Usuario {studentId} calificó incidente {id} con {dto.Rating} estrellas");

        return Ok(await MapToResponseDto(incident));
    }

    // ==================== Estadísticas del Usuario ====================
    [HttpGet("stats")]
    public async Task<IActionResult> GetMyStats()
    {
        var studentId = GetCurrentUserId();
        var myIncidents = await _incidentRepo.ListByUserAsync(studentId);

        var totalReports = myIncidents.Count;
        var openReports = myIncidents.Count(i => i.Status != "Resolved");
        var resolvedReports = myIncidents.Count(i => i.Status == "Resolved");
        
        var lastUpdate = myIncidents.Any() 
            ? myIncidents.Max(i => i.ReportedAt).ToString("yyyy-MM-dd HH:mm")
            : "N/A";

        return Ok(new
        {
            TotalReports = totalReports,
            OpenReports = openReports,
            ResolvedReports = resolvedReports,
            LastUpdate = lastUpdate
        });
    }

    // ==================== Helpers ====================
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    private async Task<TicketResponseDto> MapToResponseDto(Incident incident)
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
            Category: "General", // Los incidents no tienen categoría específica
            Priority: "Media", // Los incidents no tienen prioridad específica
            Status: status,
            Location: "N/A", // Los incidents no tienen ubicación
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
            ResolvedAt: incident.Status == "Resolved" ? incident.ReportedAt.AddHours(2) : null, // Aproximación
            SlaDeadline: null,
            SlaStatus: "-",
            Rating: null
        );
    }
}

public record RateTicketDto(int Rating, string? Comment);
