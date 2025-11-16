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
            var tickets = _tickets.Where(t => t.UserId == userId);
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
            var tickets = _tickets.Where(t => t.UserId == userId);
            return Task.FromResult<IEnumerable<Ticket>>(tickets);
        }

    }
}

