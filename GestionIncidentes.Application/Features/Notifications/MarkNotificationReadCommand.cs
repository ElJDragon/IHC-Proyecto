using MediatR;

namespace GestionIncidentes.Application.Features.Notifications;

public record MarkNotificationReadCommand(Guid NotificationId) : IRequest<Unit>;
