using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces
{
    public interface ITicketRepository
    {
        // CRUD básico
        Task AddAsync(Ticket ticket, CancellationToken ct = default);
        Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Ticket>> ListAllAsync(CancellationToken ct = default);
        Task UpdateAsync(Ticket ticket, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);

        // Consultas por usuario
        Task<IEnumerable<Ticket>> ListByUserAsync(Guid userId, CancellationToken ct = default); // Tickets creados por usuario
        Task<IEnumerable<Ticket>> ListByTechnicianAsync(Guid technicianId, CancellationToken ct = default); // Tickets asignados a técnico

        // Consultas por filtros
        Task<IEnumerable<Ticket>> ListByStatusAsync(string status, CancellationToken ct = default);
        Task<IEnumerable<Ticket>> ListByPriorityAsync(string priority, CancellationToken ct = default);
        Task<IEnumerable<Ticket>> ListByLocationAsync(string location, CancellationToken ct = default);
        Task<IEnumerable<Ticket>> SearchAsync(string searchTerm, CancellationToken ct = default);

        // Estadísticas
        Task<int> CountByStatusAsync(string status, CancellationToken ct = default);
        Task<int> CountByTechnicianAsync(Guid technicianId, CancellationToken ct = default);
        Task<int> CountTotalAsync(CancellationToken ct = default);
        Task<double> GetAverageResolutionTimeAsync(CancellationToken ct = default); // En horas

        // Consultas complejas
        Task<IEnumerable<Ticket>> GetTicketsWithSlaViolationAsync(CancellationToken ct = default);
        Task<Dictionary<string, int>> GetIncidentsByLocationAsync(CancellationToken ct = default);
        Task<IEnumerable<Ticket>> GetRecentTicketsAsync(int count, CancellationToken ct = default);
    }
}


