using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Models;
using GestionIncidentes.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionIncidentes.Web.Controllers;

[ApiController]
[Route("api/student/reports")]
[Authorize(Roles = "Student,Usuario")]
public class StudentReportsController : ControllerBase
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IUserRepository _userRepo;

    public StudentReportsController(ITicketRepository ticketRepo, IUserRepository userRepo)
    {
        _ticketRepo = ticketRepo;
        _userRepo = userRepo;
    }

    // ==================== Crear Reporte (Wizard Form) ====================
    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] CreateStudentReportDto dto)
    {
        var studentId = GetCurrentUserId();
        if (studentId == Guid.Empty)
            return Unauthorized();

        // Generar título automático basado en el tipo de problema
        var title = dto.ProblemType == "software"
            ? $"Problema con {dto.ProgramName ?? "software"} en {dto.Lab}"
            : $"Falla de hardware en {dto.Lab} - {dto.AffectedParts}";

        // Generar descripción automática
        var description = dto.ProblemType == "software"
            ? $"Programa: {dto.ProgramName}\nMensaje de error: {dto.ErrorMessage}"
            : $"Componentes afectados: {dto.AffectedParts}";

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Category = dto.ProblemType == "software" ? "software" : "hardware",
            Priority = "medium", // Por defecto, puede calcularse según el tipo
            Status = "Pendiente",
            Location = dto.Lab,
            AffectedType = "classroom",
            LocationDetail = dto.EquipmentId,
            CreatedByUserId = studentId,
            AssignedToUserId = null, // Se asigna después por workload
            ProblemType = dto.ProblemType,
            EquipmentId = dto.EquipmentId,
            ProgramName = dto.ProgramName,
            ErrorMessage = dto.ErrorMessage,
            AffectedParts = dto.AffectedParts,
            CreatedAt = DateTime.UtcNow,
            SlaDeadline = DateTime.UtcNow.AddHours(12) // SLA por defecto
        };

        await _ticketRepo.AddAsync(ticket);

        return CreatedAtAction(nameof(GetReport), new { id = ticket.Id }, await MapToResponseDto(ticket));
    }

    // ==================== Listar Mis Reportes ====================
    [HttpGet("my-reports")]
    public async Task<IActionResult> GetMyReports()
    {
        var studentId = GetCurrentUserId();
        if (studentId == Guid.Empty)
            return Unauthorized();

        var reports = await _ticketRepo.ListByUserAsync(studentId);
        
        var response = new List<TicketResponseDto>();
        foreach (var ticket in reports)
            response.Add(await MapToResponseDto(ticket));

        return Ok(response);
    }

    // ==================== Obtener Reporte por ID ====================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReport(Guid id)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Reporte no encontrado" });

        var studentId = GetCurrentUserId();
        if (ticket.CreatedByUserId != studentId)
            return Forbid();

        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Valorar Ticket Resuelto ====================
    [HttpPost("{id}/rate")]
    public async Task<IActionResult> RateTicket(Guid id, [FromBody] RateTicketDto dto)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Reporte no encontrado" });

        var studentId = GetCurrentUserId();
        if (ticket.CreatedByUserId != studentId)
            return Forbid();

        if (ticket.Status != "Resuelto")
            return BadRequest(new { message = "Solo puedes valorar tickets resueltos" });

        ticket.Rating = dto.Rating;
        ticket.FeedbackComment = dto.Comment;

        await _ticketRepo.UpdateAsync(ticket);
        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Estadísticas del Usuario ====================
    [HttpGet("stats")]
    public async Task<IActionResult> GetMyStats()
    {
        var studentId = GetCurrentUserId();
        var myReports = (await _ticketRepo.ListByUserAsync(studentId)).ToList();

        var totalReports = myReports.Count;
        var openReports = myReports.Count(t => t.Status != "Resuelto");
        var resolvedReports = myReports.Count(t => t.Status == "Resuelto");
        
        var lastUpdate = myReports.Any() 
            ? myReports.Max(t => t.CreatedAt).ToString("yyyy-MM-dd HH:mm")
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

    private async Task<TicketResponseDto> MapToResponseDto(Ticket ticket)
    {
        var createdBy = await _userRepo.GetByIdAsync(ticket.CreatedByUserId);
        var assignedTo = ticket.AssignedToUserId.HasValue 
            ? await _userRepo.GetByIdAsync(ticket.AssignedToUserId.Value)
            : null;

        var ticketNumber = $"TKT-{ticket.CreatedAt:yyMMdd}-{ticket.Id.ToString()[..4]}";

        string slaStatus = "-";
        if (ticket.SlaDeadline.HasValue && ticket.Status != "Resuelto")
        {
            var remaining = ticket.SlaDeadline.Value - DateTime.UtcNow;
            if (remaining.TotalHours > 0)
                slaStatus = $"{Math.Ceiling(remaining.TotalHours)}h";
            else
                slaStatus = "Vencido";
        }

        return new TicketResponseDto(
            Id: ticket.Id,
            TicketNumber: ticketNumber,
            Title: ticket.Title,
            Description: ticket.Description,
            Category: ticket.Category,
            Priority: ticket.Priority,
            Status: ticket.Status,
            Location: ticket.Location,
            LocationDetail: ticket.LocationDetail,
            ProblemType: ticket.ProblemType,
            ProgramName: ticket.ProgramName,
            ErrorMessage: ticket.ErrorMessage,
            AffectedParts: ticket.AffectedParts,
            CreatedByUserId: ticket.CreatedByUserId,
            CreatedByName: createdBy?.FullName ?? "Desconocido",
            AssignedToUserId: ticket.AssignedToUserId,
            AssignedToName: assignedTo?.FullName,
            TechnicianNotes: ticket.TechnicianNotes,
            CreatedAt: ticket.CreatedAt,
            ResolvedAt: ticket.ResolvedAt,
            SlaDeadline: ticket.SlaDeadline,
            SlaStatus: slaStatus,
            Rating: ticket.Rating
        );
    }
}

public record RateTicketDto(int Rating, string? Comment);
