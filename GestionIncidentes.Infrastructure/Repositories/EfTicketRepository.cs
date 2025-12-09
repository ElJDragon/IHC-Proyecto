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

        // ==================== CRUD Básico ====================
        public async Task AddAsync(Ticket ticket, CancellationToken ct = default)
        {
            if (ticket.Id == Guid.Empty)
                ticket.Id = Guid.NewGuid();

            await _context.Tickets.AddAsync(ticket, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        public async Task<IEnumerable<Ticket>> ListAllAsync(CancellationToken ct = default)
        {
            return await _context.Tickets
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task UpdateAsync(Ticket ticket, CancellationToken ct = default)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync(ct);
            }
        }

        // ==================== Consultas por Usuario ====================
        public async Task<IEnumerable<Ticket>> ListByUserAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Tickets
                .Where(t => t.CreatedByUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<List<Ticket>> GetByCreatedByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Tickets
                .Where(t => t.CreatedByUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Ticket>> ListByTechnicianAsync(Guid technicianId, CancellationToken ct = default)
        {
            return await _context.Tickets
                .Where(t => t.AssignedToUserId == technicianId)
                .OrderBy(t => t.SlaDeadline)
                .ToListAsync(ct);
        }

        // ==================== Consultas por Filtros ====================
        public async Task<IEnumerable<Ticket>> ListByStatusAsync(string status, CancellationToken ct = default)
        {
            return await _context.Tickets
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Ticket>> ListByPriorityAsync(string priority, CancellationToken ct = default)
        {
            return await _context.Tickets
                .Where(t => t.Priority == priority)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Ticket>> ListByLocationAsync(string location, CancellationToken ct = default)
        {
            return await _context.Tickets
                .Where(t => t.Location.Contains(location))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Ticket>> SearchAsync(string searchTerm, CancellationToken ct = default)
        {
            return await _context.Tickets
                .Where(t => t.Title.Contains(searchTerm) || 
                           t.Description.Contains(searchTerm) ||
                           t.Location.Contains(searchTerm))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }

        // ==================== Estadísticas ====================
        public async Task<int> CountByStatusAsync(string status, CancellationToken ct = default)
        {
            return await _context.Tickets.CountAsync(t => t.Status == status, ct);
        }

        public async Task<int> CountByTechnicianAsync(Guid technicianId, CancellationToken ct = default)
        {
            return await _context.Tickets.CountAsync(t => t.AssignedToUserId == technicianId, ct);
        }

        public async Task<int> CountTotalAsync(CancellationToken ct = default)
        {
            return await _context.Tickets.CountAsync(ct);
        }

        public async Task<double> GetAverageResolutionTimeAsync(CancellationToken ct = default)
        {
            var resolvedTickets = await _context.Tickets
                .Where(t => t.Status == "Resuelto" && t.ResolvedAt.HasValue)
                .ToListAsync(ct);

            if (!resolvedTickets.Any())
                return 0;

            var totalHours = resolvedTickets
                .Select(t => (t.ResolvedAt!.Value - t.CreatedAt).TotalHours)
                .Average();

            return Math.Round(totalHours, 1);
        }

        // ==================== Consultas Complejas ====================
        public async Task<IEnumerable<Ticket>> GetTicketsWithSlaViolationAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            return await _context.Tickets
                .Where(t => t.Status != "Resuelto" && 
                           t.SlaDeadline.HasValue && 
                           t.SlaDeadline.Value < now)
                .OrderBy(t => t.SlaDeadline)
                .ToListAsync(ct);
        }

        public async Task<Dictionary<string, int>> GetIncidentsByLocationAsync(CancellationToken ct = default)
        {
            return await _context.Tickets
                .GroupBy(t => t.Location)
                .Select(g => new { Location = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Location, x => x.Count, ct);
        }

        public async Task<IEnumerable<Ticket>> GetRecentTicketsAsync(int count, CancellationToken ct = default)
        {
            return await _context.Tickets
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .ToListAsync(ct);
        }
    }
}

