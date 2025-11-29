using System;

namespace GestionIncidentes.Domain.Entities;

/// <summary>
/// Registro de auditoría para tracking de acciones importantes
/// </summary>
public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Action { get; set; } = string.Empty; // "CreatedTicket", "AssignedTicket", "ResolvedTicket", etc.
    public string EntityType { get; set; } = string.Empty; // "Ticket", "KnowledgeEntry", "User", etc.
    public Guid? EntityId { get; set; }
    public string Details { get; set; } = string.Empty; // JSON con detalles adicionales
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = string.Empty;

    private AuditLog() { }

    public static AuditLog Create(
        Guid userId,
        string action,
        string entityType,
        Guid? entityId = null,
        string details = "",
        string ipAddress = "")
    {
        return new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            IpAddress = ipAddress
        };
    }
}
