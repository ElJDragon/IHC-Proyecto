using MediatR;

namespace GestionIncidentes.Application.Features.Notifications;

public record GetNotificationsQuery(Guid UserId) : IRequest<List<NotificationDto>>;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? RelatedEntityId { get; set; }
}
