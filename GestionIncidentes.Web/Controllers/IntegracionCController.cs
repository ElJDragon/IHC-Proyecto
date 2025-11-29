using GestionIncidentes.Application.Features.Knowledge;
using GestionIncidentes.Application.Features.Notifications;
using GestionIncidentes.Application.Features.Reports;
using GestionIncidentes.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionIncidentes.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KnowledgeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public KnowledgeController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Crear una nueva entrada en la Base de Conocimiento
    /// </summary>
    [HttpPost("entries")]
    public async Task<ActionResult<Guid>> CreateKnowledgeEntry(
        [FromBody] CreateKnowledgeEntryRequest request)
    {
        var command = new AddKnowledgeEntryCommand(
            request.Title,
            request.Problem,
            request.Solution,
            request.Category,
            _currentUser.UserId ?? Guid.NewGuid(),
            request.RelatedTicketId,
            request.Tags
        );

        var entryId = await _mediator.Send(command);
        return Ok(new { id = entryId, message = "Entrada creada exitosamente" });
    }

    /// <summary>
    /// Buscar entradas en la Base de Conocimiento
    /// </summary>
    [HttpGet("entries/search")]
    public async Task<ActionResult<List<KnowledgeEntryDto>>> SearchKnowledgeEntries(
        [FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return BadRequest(new { error = "El término de búsqueda es requerido" });

        var query = new SearchKnowledgeQuery(searchTerm);
        var results = await _mediator.Send(query);
        
        return Ok(results);
    }

    /// <summary>
    /// Obtener notificaciones del usuario actual
    /// </summary>
    [HttpGet("notifications")]
    public async Task<ActionResult<List<NotificationDto>>> GetNotifications()
    {
        var userId = _currentUser.UserId ?? Guid.NewGuid();
        var query = new GetNotificationsQuery(userId);
        var notifications = await _mediator.Send(query);
        
        return Ok(notifications);
    }

    /// <summary>
    /// Marcar una notificación como leída
    /// </summary>
    [HttpPut("notifications/{notificationId}/read")]
    public async Task<ActionResult> MarkNotificationAsRead(Guid notificationId)
    {
        var command = new MarkNotificationReadCommand(notificationId);
        await _mediator.Send(command);
        
        return Ok(new { message = "Notificación marcada como leída" });
    }

    /// <summary>
    /// Generar un reporte de ticket
    /// </summary>
    [HttpPost("reports")]
    public async Task<ActionResult<Guid>> GenerateTicketReport(
        [FromBody] GenerateTicketReportRequest request)
    {
        var command = new GenerateTicketReportQuery(
            request.TicketId,
            _currentUser.UserId ?? Guid.NewGuid(),
            request.ResolutionDetails,
            request.ProblemDiagnosis,
            request.ActionsTaken,
            TimeSpan.FromMinutes(request.TimeSpentMinutes),
            request.SuggestKnowledgeEntry
        );

        var reportId = await _mediator.Send(command);
        return Ok(new { id = reportId, message = "Reporte generado exitosamente" });
    }
}

// DTOs para requests
public class CreateKnowledgeEntryRequest
{
    public string Title { get; set; } = string.Empty;
    public string Problem { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Guid? RelatedTicketId { get; set; }
    public List<string>? Tags { get; set; }
}

public class GenerateTicketReportRequest
{
    public Guid TicketId { get; set; }
    public string ResolutionDetails { get; set; } = string.Empty;
    public string ProblemDiagnosis { get; set; } = string.Empty;
    public string ActionsTaken { get; set; } = string.Empty;
    public int TimeSpentMinutes { get; set; }
    public bool SuggestKnowledgeEntry { get; set; }
}
