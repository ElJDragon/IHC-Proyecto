using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Models;
using GestionIncidentes.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIncidentes.Web.Controllers
{
    [ApiController]
    [Route("api/admin/tickets")]
    [Authorize] // Todos los endpoints requieren token
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepo;

        public TicketsController(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        // -------------------- Crear ticket --------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
        {
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(), // Generado automáticamente
                Title = dto.Title,
                Description = dto.Description,
                CreatedByUserId=dto.CreatedByUserId,
                Status = "Pendiente",
                CreatedAt = DateTime.UtcNow
            };

            await _ticketRepo.AddAsync(ticket);
            return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticket);
        }

        // -------------------- Listar todos los tickets --------------------
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tickets = await _ticketRepo.ListAllAsync();
            return Ok(tickets);
        }

        // -------------------- Obtener ticket por ID --------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var ticket = await _ticketRepo.GetByIdAsync(id);
            if (ticket == null)
                return NotFound(new { message = "Ticket not found" });

            return Ok(ticket);
        }

        // -------------------- DTO para actualización --------------------
        public record UpdateTicketDto(string Title, string Description, Guid UserId);

        // -------------------- Actualizar ticket --------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketDto dto)
        {
            var existingTicket = await _ticketRepo.GetByIdAsync(id);
            if (existingTicket == null)
                return NotFound(new { message = "Ticket not found" });

            // Actualizar propiedades
            existingTicket.Title = dto.Title;
            existingTicket.Description = dto.Description;

            await _ticketRepo.UpdateAsync(existingTicket);
            return NoContent();
        }

        // -------------------- Eliminar ticket --------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingTicket = await _ticketRepo.GetByIdAsync(id);
            if (existingTicket == null)
                return NotFound(new { message = "Ticket not found" });

            await _ticketRepo.DeleteAsync(id);
            return NoContent();
        }

        // -------------------- Listar tickets por usuario --------------------
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> ListByUser(Guid userId)
        {
            var tickets = await _ticketRepo.ListByUserAsync(userId);
            return Ok(tickets);
        }
    }
}
