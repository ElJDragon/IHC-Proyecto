using System;

namespace GestionIncidentes.Domain.Entities;

/// <summary>
/// Notificación del sistema
/// </summary>
public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "IncidentReported", "TicketAssigned", "TicketResolved", etc.
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? RelatedEntityId { get; set; } // ID del ticket o incidente relacionado

    private Notification() { }

    public static Notification Create(Guid userId, string title, string message, string type, Guid? relatedEntityId = null)
    {
        return new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            RelatedEntityId = relatedEntityId
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
