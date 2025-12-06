using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces;

public interface IIncidentRepository
{
    Task<Incident?> GetByIdAsync(Guid id);
    Task<List<Incident>> ListAllAsync();
    Task<List<Incident>> ListByUserAsync(Guid userId);
    Task AddAsync(Incident incident);
    Task UpdateAsync(Incident incident);
    Task DeleteAsync(Guid id);
}
