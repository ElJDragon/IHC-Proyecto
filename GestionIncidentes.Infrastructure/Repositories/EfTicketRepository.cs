using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure.Repositories
{
    public class EfTicketRepository : ITicketRepository
    {
        private readonly GestionIncidentesDbContext _context;

        public EfTicketRepository(GestionIncidentesDbContext context)
        {
            _context = context;
        }

        // Crear ticket
        public async Task AddAsync(Ticket ticket, CancellationToken ct = default)
        {
            if (ticket.Id == Guid.Empty)
                ticket.Id = Guid.NewGuid();

            await _context.Tickets.AddAsync(ticket, ct);
            await _context.SaveChangesAsync(ct);
        }

        // Obtener ticket por Id
        public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        // Listar todos los tickets
        public async Task<IEnumerable<Ticket>> ListAllAsync(CancellationToken ct = default)
        {
            return await _context.Tickets.ToListAsync(ct);
        }

        // Listar todos los tickets (sobrecarga sin parámetros)
        public async Task<IEnumerable<Ticket>> ListAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        // Listar tickets por usuario
        public async Task<IEnumerable<Ticket>> ListByUserAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Tickets.Where(t => t.UserId == userId).ToListAsync(ct);
        }

        // Actualizar ticket
        public async Task UpdateAsync(Ticket ticket, CancellationToken ct = default)
        {
            var existing = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticket.Id, ct);
            if (existing != null)
            {
                existing.Title = ticket.Title;
                existing.Description = ticket.Description;
                existing.Status = ticket.Status;
                existing.UserId = ticket.UserId;
                // Agrega otras propiedades si las hay
                _context.Tickets.Update(existing);
                await _context.SaveChangesAsync(ct);
            }
        }

        // Eliminar ticket
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
