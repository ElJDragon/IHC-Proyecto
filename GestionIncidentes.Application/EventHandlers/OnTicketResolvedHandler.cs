using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.EventHandlers;

/// <summary>
/// Maneja el evento OnTicketResolved para generar borrador de reporte y sugerir KnowledgeEntry
/// </summary>
public class OnTicketResolvedHandler : INotificationHandler<TicketResolved>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ITicketRepository _ticketRepository;

    public OnTicketResolvedHandler(
        INotificationRepository notificationRepository,
        ITicketRepository ticketRepository)
    {
        _notificationRepository = notificationRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task Handle(TicketResolved notification, CancellationToken cancellationToken)
    {
        // Obtener el ticket para saber quién lo resolvió
        var ticket = await _ticketRepository.GetByIdAsync(notification.TicketId);
        if (ticket == null) return;
        
        // Notificar al técnico asignado para que genere el reporte
        var notif = Notification.Create(
            ticket.UserId,
            "Ticket Resuelto - Generar Reporte",
            $"Has resuelto el ticket. Por favor genera el reporte correspondiente y considera agregarlo a la Base de Conocimiento.",
            "TicketResolved",
            notification.TicketId
        );

        await _notificationRepository.AddAsync(notif);

        // También notificar al creador del ticket
        if (ticket.CreatedByUserId != ticket.UserId)
        {
            var creatorNotif = Notification.Create(
                ticket.CreatedByUserId,
                "Tu Ticket ha sido Resuelto",
                $"El ticket '{ticket.Title}' ha sido resuelto.",
                "TicketResolved",
                notification.TicketId
            );

            await _notificationRepository.AddAsync(creatorNotif);
        }
    }
}
