using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Infrastructure.Repositories
{
    public class InMemoryTicketRepository : ITicketRepository
    {
        private readonly List<Ticket> _tickets = new();

        // Crear ticket
        // Crear ticket
        public Task AddAsync(Ticket ticket, CancellationToken ct = default)
        {
            // Generar un Id nuevo si no tiene
            if (ticket.Id == Guid.Empty)
                ticket.Id = Guid.NewGuid();

            _tickets.Add(ticket);
            return Task.CompletedTask;
        }

        // Obtener ticket por Id
        public Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return Task.FromResult(_tickets.FirstOrDefault(t => t.Id == id));
        }

        // Listar todos los tickets
        public Task<IEnumerable<Ticket>> ListAllAsync(CancellationToken ct = default)
        {
            return Task.FromResult<IEnumerable<Ticket>>(_tickets);
        }

        // Listar todos los tickets (sobrecarga sin parámetros)
        public Task<IEnumerable<Ticket>> ListAllAsync()
        {
            return Task.FromResult<IEnumerable<Ticket>>(_tickets);
        }

        // Listar tickets por usuario
        public Task<IEnumerable<Ticket>> ListByUserAsync(Guid userId)
        {
            var tickets = _tickets.Where(t => t.CreatedByUserId == userId);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

        // Actualizar ticket
        public Task UpdateAsync(Ticket ticket, CancellationToken ct = default)
        {
            var index = _tickets.FindIndex(t => t.Id == ticket.Id);
            if (index != -1)
                _tickets[index] = ticket;

            return Task.CompletedTask;
        }

        // Eliminar ticket
        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket != null)
                _tickets.Remove(ticket);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Ticket>> ListByUserAsync(Guid userId, CancellationToken ct = default)
        {
            var tickets = _tickets.Where(t => t.CreatedByUserId == userId);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

        public Task<List<Ticket>> GetByCreatedByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var tickets = _tickets.Where(t => t.CreatedByUserId == userId).ToList();
            return Task.FromResult(tickets);
        }

        public Task<IEnumerable<Ticket>> ListByTechnicianAsync(Guid technicianId, CancellationToken ct = default)
        {
            var tickets = _tickets
                .Where(t => t.AssignedToUserId == technicianId)
                .OrderBy(t => t.SlaDeadline);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

        public Task<IEnumerable<Ticket>> ListByStatusAsync(string status, CancellationToken ct = default)
        {
            var tickets = _tickets
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

        public Task<IEnumerable<Ticket>> ListByPriorityAsync(string priority, CancellationToken ct = default)
        {
            var tickets = _tickets
                .Where(t => t.Priority == priority)
                .OrderByDescending(t => t.CreatedAt);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

        public Task<IEnumerable<Ticket>> ListByLocationAsync(string location, CancellationToken ct = default)
        {
            var tickets = _tickets
                .Where(t => t.Location == location)
                .OrderByDescending(t => t.CreatedAt);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

        public Task<IEnumerable<Ticket>> SearchAsync(string searchTerm, CancellationToken ct = default)
        {
            var tickets = _tickets
                .Where(t => 
                    t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (t.Location != null && t.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(t => t.CreatedAt);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

        public Task<int> CountByStatusAsync(string status, CancellationToken ct = default)
        {
            return Task.FromResult(_tickets.Count(t => t.Status == status));
        }

        public Task<int> CountByTechnicianAsync(Guid technicianId, CancellationToken ct = default)
        {
            return Task.FromResult(_tickets.Count(t => t.AssignedToUserId == technicianId));
        }

        public Task<int> CountTotalAsync(CancellationToken ct = default)
        {
            return Task.FromResult(_tickets.Count);
        }

        public Task<double> GetAverageResolutionTimeAsync(CancellationToken ct = default)
        {
            var resolvedTickets = _tickets
                .Where(t => t.Status == "Resuelto" && t.ResolvedAt != null)
                .ToList();
            
            if (!resolvedTickets.Any())
                return Task.FromResult(0.0);

            var avgHours = resolvedTickets.Average(t => (t.ResolvedAt!.Value - t.CreatedAt).TotalHours);
            return Task.FromResult(avgHours);
        }

        public Task<IEnumerable<Ticket>> GetTicketsWithSlaViolationAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            var tickets = _tickets
                .Where(t => t.SlaDeadline != null && 
                           now > t.SlaDeadline && 
                           t.Status != "Resuelto")
                .OrderBy(t => t.SlaDeadline);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

        public Task<Dictionary<string, int>> GetIncidentsByLocationAsync(CancellationToken ct = default)
        {
            var result = _tickets
                .Where(t => !string.IsNullOrEmpty(t.Location))
                .GroupBy(t => t.Location!)
                .ToDictionary(g => g.Key, g => g.Count());
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Ticket>> GetRecentTicketsAsync(int count, CancellationToken ct = default)
        {
            var tickets = _tickets
                .OrderByDescending(t => t.CreatedAt)
                .Take(count);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }
    }
}

