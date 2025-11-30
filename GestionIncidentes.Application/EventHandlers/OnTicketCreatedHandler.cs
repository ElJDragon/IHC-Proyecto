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
        // El evento TicketCreated ya no tiene AssignedToUserId
        // La notificación se enviará cuando se asigne el ticket (evento TicketAssigned)
        
        // Opcionalmente, notificar al creador que el ticket fue creado
        var notif = Notification.Create(
            notification.CreatedByUserId,
            "Ticket Creado",
            $"Tu ticket '{notification.Title}' ha sido creado exitosamente.",
            "TicketCreated",
            notification.TicketId
        );

        await _notificationRepository.AddAsync(notif);
    }
}
