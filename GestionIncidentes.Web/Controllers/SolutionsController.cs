using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionIncidentes.Infrastructure;
using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SolutionsController : ControllerBase
{
    private readonly GestionIncidentesDbContext _context;
    private readonly ILogger<SolutionsController> _logger;

    public SolutionsController(GestionIncidentesDbContext context, ILogger<SolutionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSolution([FromBody] CreateSolutionDto dto)
    {
        try
        {
            // Validar que el ticket existe
            var ticket = await _context.Tickets.FindAsync(dto.TicketId);
            if (ticket == null)
            {
                return NotFound("Ticket no encontrado");
            }

            // Crear la solución usando el método factory
            var solution = Solution.Create(
                dto.TicketId,
                dto.Title,
                dto.Description,
                dto.CreatedByUserId
            );

            // Marcar como efectiva y completada
            solution.MarkAsEffective();

            // Si se usa una entrada de KB, vincularla
            if (dto.KnowledgeEntryId.HasValue)
            {
                solution.PromoteToKnowledgeBase(dto.KnowledgeEntryId.Value);
            }

            _context.Solutions.Add(solution);

            // Agregar pasos si los hay (solo si NO se usa una entrada de KB)
            if (!dto.KnowledgeEntryId.HasValue && dto.Steps != null && dto.Steps.Any())
            {
                foreach (var stepDto in dto.Steps)
                {
                    var step = SolutionStep.Create(
                        solution.Id,
                        stepDto.StepNumber,
                        stepDto.Title,
                        stepDto.Description
                    );
                    _context.SolutionSteps.Add(step);
                }
            }

            // Si se debe agregar a la base de conocimiento
            if (dto.AddToKnowledgeBase && !dto.KnowledgeEntryId.HasValue)
            {
                var knowledgeEntry = KnowledgeEntry.Create(
                    dto.Title,
                    ticket.Description,
                    dto.Description,
                    ticket.Category,
                    dto.CreatedByUserId,
                    dto.TicketId,
                    new List<string> { ticket.Category, ticket.Priority }
                );

                _context.KnowledgeEntries.Add(knowledgeEntry);
            }

            // Si se usa una entrada de KB existente, incrementar su contador
            if (dto.KnowledgeEntryId.HasValue)
            {
                var knowledgeEntry = await _context.KnowledgeEntries.FindAsync(dto.KnowledgeEntryId.Value);
                if (knowledgeEntry != null)
                {
                    knowledgeEntry.UsageCount++;
                    knowledgeEntry.UpdatedAt = DateTime.UtcNow;
                }
            }

            // Actualizar el estado del ticket
            ticket.Status = "Resuelto";
            ticket.ResolvedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Solución creada exitosamente para ticket {TicketId}", dto.TicketId);
            return Ok(new { message = "Solución guardada exitosamente", solutionId = solution.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la solución");
            return StatusCode(500, "Error al guardar la solución");
        }
    }
}

public class CreateSolutionDto
{
    public Guid TicketId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public Guid CreatedByUserId { get; set; }
    public bool AddToKnowledgeBase { get; set; }
    public List<SolutionStepDto> Steps { get; set; } = new();
    public Guid? KnowledgeEntryId { get; set; }
}

public class SolutionStepDto
{
    public int StepNumber { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}
