using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Infrastructure.Services;

/// <summary>
/// Servicio de notificaciones en memoria (puede ser reemplazado por SignalR o correo)
/// </summary>
public class NotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task NotifyUserAsync(Guid userId, string title, string message, string type, Guid? relatedEntityId = null)
    {
        var notification = Notification.Create(userId, title, message, type, relatedEntityId);
        await _notificationRepository.AddAsync(notification);
    }

    public async Task NotifyMultipleUsersAsync(List<Guid> userIds, string title, string message, string type, Guid? relatedEntityId = null)
    {
        foreach (var userId in userIds)
        {
            await NotifyUserAsync(userId, title, message, type, relatedEntityId);
        }
    }
}
