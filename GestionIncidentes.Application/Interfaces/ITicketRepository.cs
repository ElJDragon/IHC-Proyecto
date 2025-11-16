using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces
{
    public interface ITicketRepository
    {
        // Crear ticket
        Task AddAsync(Ticket ticket, CancellationToken ct = default);

        // Obtener ticket por Id
        Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // Listar todos los tickets
        Task<IEnumerable<Ticket>> ListAllAsync(CancellationToken ct = default);

        // Listar tickets por usuario
        Task<IEnumerable<Ticket>> ListByUserAsync(Guid userId, CancellationToken ct = default);

        // Actualizar ticket
        Task UpdateAsync(Ticket ticket, CancellationToken ct = default);

        // Eliminar ticket
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}

