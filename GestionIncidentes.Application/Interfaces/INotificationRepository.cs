using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(Guid id);
    Task<List<Notification>> GetByUserIdAsync(Guid userId);
    Task<List<Notification>> GetUnreadByUserIdAsync(Guid userId);
    Task AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task DeleteAsync(Guid id);
}
