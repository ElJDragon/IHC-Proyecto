using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionIncidentes.Web.Controllers;

[ApiController]
[Route("api/technician/tickets")]
[Authorize(Roles = "Technician,Tecnico")]
public class TechnicianTicketsController : ControllerBase
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IUserRepository _userRepo;

    public TechnicianTicketsController(ITicketRepository ticketRepo, IUserRepository userRepo)
    {
        _ticketRepo = ticketRepo;
        _userRepo = userRepo;
    }

    // ==================== Listar Mis Tickets Asignados ====================
    [HttpGet("my-tickets/{id}")]
    public async Task<IActionResult> GetMyTickets(Guid id)
    {
       
        if (id == Guid.Empty)
            return Unauthorized();

        var tickets = await _ticketRepo.ListByTechnicianAsync(id);
        
        var response = new List<TicketResponseDto>();
        foreach (var ticket in tickets)
            response.Add(await MapToResponseDto(ticket));

        return Ok(response);
    }

    // ==================== Obtener Ticket por ID ====================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(Guid id)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        // Verificar que el ticket esté asignado al técnico actual
        var technicianId = GetCurrentUserId();
        if (ticket.AssignedToUserId != technicianId)
            return Forbid();

        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Cambiar Estado del Ticket ====================
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeTicketStatusDto dto)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        var technicianId = GetCurrentUserId();
        if (ticket.AssignedToUserId != technicianId)
            return Forbid();

        ticket.Status = dto.NewStatus;
        if (dto.TechnicianNotes != null)
            ticket.TechnicianNotes = dto.TechnicianNotes;

        await _ticketRepo.UpdateAsync(ticket);
        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Reasignar Ticket ====================
    [HttpPatch("{id}/reassign")]
    public async Task<IActionResult> ReassignTicket(Guid id, [FromBody] ReassignTicketDto dto)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        var technicianId = GetCurrentUserId();
        if (ticket.AssignedToUserId != technicianId)
            return Forbid();

        ticket.AssignedToUserId = dto.NewTechnicianId;
        ticket.TechnicianNotes = (ticket.TechnicianNotes ?? "") + $"\n[Reasignado a otro técnico el {DateTime.UtcNow:yyyy-MM-dd HH:mm}]";

        await _ticketRepo.UpdateAsync(ticket);
        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Cerrar Ticket ====================
    [HttpPost("{id}/close")]
    public async Task<IActionResult> CloseTicket(Guid id, [FromBody] CloseTicketDto dto)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        var technicianId = GetCurrentUserId();
        if (ticket.AssignedToUserId != technicianId)
            return Forbid();

        ticket.Status = "Resuelto";
        ticket.TechnicianNotes = dto.TechnicianNotes;
        ticket.ResolvedAt = DateTime.UtcNow;
        if (dto.Rating.HasValue)
            ticket.Rating = dto.Rating;

        await _ticketRepo.UpdateAsync(ticket);
        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Actualizar Bitácora ====================
    [HttpPatch("{id}/notes")]
    public async Task<IActionResult> UpdateNotes(Guid id, [FromBody] UpdateNotesDto dto)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        var technicianId = GetCurrentUserId();
        if (ticket.AssignedToUserId != technicianId)
            return Forbid();

        ticket.TechnicianNotes = dto.Notes;
        await _ticketRepo.UpdateAsync(ticket);
        
        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Estadísticas del Técnico ====================
    [HttpGet("stats")]
    public async Task<IActionResult> GetMyStats()
    {
        var technicianId = GetCurrentUserId();
        var myTickets = (await _ticketRepo.ListByTechnicianAsync(technicianId)).ToList();

        var pendingCount = myTickets.Count(t => t.Status == "Pendiente");
        var inProgressCount = myTickets.Count(t => t.Status == "En proceso");
        var resolvedCount = myTickets.Count(t => t.Status == "Resuelto");

        var resolvedTickets = myTickets.Where(t => t.Status == "Resuelto" && t.ResolvedAt.HasValue).ToList();
        var avgTime = resolvedTickets.Any()
            ? resolvedTickets.Average(t => (t.ResolvedAt!.Value - t.CreatedAt).TotalHours)
            : 0;

        var stats = new TechnicianStatsDto(
            AssignedTickets: myTickets.Count,
            PendingTickets: pendingCount,
            InProgressTickets: inProgressCount,
            ResolvedTickets: resolvedCount,
            AverageResolutionTime: Math.Round(avgTime, 1)
        );

        return Ok(stats);
    }

    // ==================== Helpers ====================
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    private async Task<TicketResponseDto> MapToResponseDto(Domain.Entities.Ticket ticket)
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

public record ReassignTicketDto(Guid NewTechnicianId);
public record UpdateNotesDto(string Notes);
