using GestionIncidentes.Application.Features.Incidents.Commands;
using GestionIncidentes.Application.Features.Incidents.Dtos;
using GestionIncidentes.Application.Features.Incidents.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIncidentes.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IncidentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public IncidentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Reportar un nuevo incidente
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ReportIncident([FromBody] ReportIncidentDto dto)
    {
        var command = new ReportIncidentCommand(dto.Title, dto.Description);
        var incidentId = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetIncidentDetails), new { id = incidentId }, new { id = incidentId });
    }

    /// <summary>
    /// Obtener detalles de un incidente específico
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIncidentDetails(Guid id)
    {
        var query = new GetIncidentDetailsQuery(id);
        var incident = await _mediator.Send(query);

        if (incident == null)
            return NotFound(new { message = "Incidente no encontrado" });

        return Ok(incident);
    }

    /// <summary>
    /// Listar todos los incidentes
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListIncidents()
    {
        var query = new ListIncidentsQuery();
        var incidents = await _mediator.Send(query);
        
        return Ok(incidents);
    }
}
