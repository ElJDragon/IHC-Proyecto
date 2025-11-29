using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Notifications;

public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, Unit>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationReadCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Unit> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
        
        if (notification != null)
        {
            notification.MarkAsRead();
            await _notificationRepository.UpdateAsync(notification);
        }

        return Unit.Value;
    }
}
