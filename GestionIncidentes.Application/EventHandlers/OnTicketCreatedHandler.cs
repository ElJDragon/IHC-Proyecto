using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.EventHandlers;

/// <summary>
/// Maneja el evento OnTicketCreated para notificar al asignado
/// </summary>
public class OnTicketCreatedHandler : INotificationHandler<TicketCreated>
{
    private readonly INotificationRepository _notificationRepository;

    public OnTicketCreatedHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(TicketCreated notification, CancellationToken cancellationToken)
    {
        if (notification.AssignedToUserId.HasValue)
        {
            var notif = Notification.Create(
                notification.AssignedToUserId.Value,
                "Ticket Asignado",
                $"Se te ha asignado un nuevo ticket.",
                "TicketAssigned",
                notification.TicketId
            );

            await _notificationRepository.AddAsync(notif);
        }
    }
}
