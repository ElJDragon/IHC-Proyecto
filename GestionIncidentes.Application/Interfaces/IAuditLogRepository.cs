using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces;

public interface IAuditLogRepository
{
    Task<AuditLog?> GetByIdAsync(Guid id);
    Task<List<AuditLog>> GetByUserIdAsync(Guid userId);
    Task<List<AuditLog>> GetByEntityAsync(string entityType, Guid entityId);
    Task<List<AuditLog>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<List<AuditLog>> GetAllAsync();
    Task AddAsync(AuditLog log);
}
