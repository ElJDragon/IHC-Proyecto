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
        // Notificar al técnico que resolvió para que genere el reporte
        var notif = Notification.Create(
            notification.ResolvedByUserId,
            "Ticket Resuelto - Generar Reporte",
            $"Has resuelto el ticket. Por favor genera el reporte correspondiente y considera agregarlo a la Base de Conocimiento.",
            "TicketResolved",
            notification.TicketId
        );

        await _notificationRepository.AddAsync(notif);

        // También notificar al creador del ticket
        var ticket = await _ticketRepository.GetByIdAsync(notification.TicketId);
        if (ticket != null)
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
