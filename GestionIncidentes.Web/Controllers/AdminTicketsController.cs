using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Models;
using GestionIncidentes.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIncidentes.Web.Controllers;

[ApiController]
[Route("api/admin/tickets")]
[Authorize(Roles = "Admin")]
public class AdminTicketsController : ControllerBase
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IUserRepository _userRepo;

    public AdminTicketsController(ITicketRepository ticketRepo, IUserRepository userRepo)
    {
        _ticketRepo = ticketRepo;
        _userRepo = userRepo;
    }

    // ==================== Crear Nuevo Incidente ====================
    [HttpPost]
    public async Task<IActionResult> CreateIncident([FromBody] CreateAdminTicketDto dto)
    {
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            Priority = dto.Priority,
            Status = "Pendiente",
            Location = dto.Location,
            AffectedType = dto.AffectedType,
            LocationDetail = dto.LocationDetail,
            CreatedByUserId = dto.CreatedByUserId,
            AssignedToUserId = dto.AssignedToUserId,
            CreatedAt = DateTime.UtcNow,
            SlaDeadline = CalculateSlaDeadline(dto.Priority)
        };

        await _ticketRepo.AddAsync(ticket);

        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, await MapToResponseDto(ticket));
    }

    // ==================== Listar Todos los Tickets (con filtros) ====================
    [HttpGet]
    public async Task<IActionResult> ListTickets(
        [FromQuery] string? technician,
        [FromQuery] string? status,
        [FromQuery] string? priority,
        [FromQuery] string? search)
    {
        var tickets = await _ticketRepo.ListAllAsync();

        // Aplicar filtros
        if (!string.IsNullOrEmpty(status) && status != "all")
            tickets = tickets.Where(t => t.Status == status);

        if (!string.IsNullOrEmpty(priority) && priority != "all")
            tickets = tickets.Where(t => t.Priority == priority);

        if (!string.IsNullOrEmpty(technician) && technician != "all")
        {
            var techUser = (await _userRepo.ListAllAsync()).FirstOrDefault(u => u.FullName == technician);
            if (techUser != null)
                tickets = tickets.Where(t => t.AssignedToUserId == techUser.Id);
        }

        if (!string.IsNullOrEmpty(search))
            tickets = tickets.Where(t => 
                t.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                t.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));

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

        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Actualizar Ticket ====================
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] UpdateTicketDto dto)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        if (dto.Title != null) ticket.Title = dto.Title;
        if (dto.Description != null) ticket.Description = dto.Description;
        if (dto.Category != null) ticket.Category = dto.Category;
        if (dto.Priority != null) ticket.Priority = dto.Priority;
        if (dto.Status != null) ticket.Status = dto.Status;
        if (dto.AssignedToUserId.HasValue) ticket.AssignedToUserId = dto.AssignedToUserId;
        if (dto.TechnicianNotes != null) ticket.TechnicianNotes = dto.TechnicianNotes;

        await _ticketRepo.UpdateAsync(ticket);
        return Ok(await MapToResponseDto(ticket));
    }

    // ==================== Eliminar Ticket ====================
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null)
            return NotFound(new { message = "Ticket no encontrado" });

        await _ticketRepo.DeleteAsync(id);
        return NoContent();
    }

    // ==================== Dashboard Stats ====================
    [HttpGet("stats/dashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var allTickets = (await _ticketRepo.ListAllAsync()).ToList();
        var labIncidents = await _ticketRepo.GetIncidentsByLocationAsync();
        var avgTime = await _ticketRepo.GetAverageResolutionTimeAsync();

        var totalIncidents = allTickets.Count;
        var criticalLab = labIncidents.OrderByDescending(x => x.Value).FirstOrDefault();
        
        // Calcular eficiencia (% de tickets con SLA cumplido)
        var resolvedWithSla = allTickets.Count(t => 
            t.Status == "Resuelto" && 
            t.ResolvedAt.HasValue && 
            t.SlaDeadline.HasValue && 
            t.ResolvedAt.Value <= t.SlaDeadline.Value);
        var totalResolved = allTickets.Count(t => t.Status == "Resuelto");
        var efficiency = totalResolved > 0 ? (double)resolvedWithSla / totalResolved * 100 : 100;

        var stats = new AdminDashboardStatsDto(
            TotalIncidents: totalIncidents,
            AvgResolutionTime: avgTime,
            CriticalLab: criticalLab.Key ?? "N/A",
            CriticalLabCount: criticalLab.Value,
            DepartmentEfficiency: Math.Round(efficiency, 0),
            LabIncidents: labIncidents.Select(x => new LabIncidentDto(
                Location: x.Key,
                Count: x.Value
            )).ToList()
        );

        return Ok(stats);
    }

    // ==================== Helpers ====================
    private DateTime CalculateSlaDeadline(string priority)
    {
        var now = DateTime.UtcNow;
        return priority switch
        {
            "critical" => now.AddHours(2),
            "high" => now.AddHours(6),
            "medium" => now.AddHours(12),
            "low" => now.AddHours(24),
            _ => now.AddHours(12)
        };
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
