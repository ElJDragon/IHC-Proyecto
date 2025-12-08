using System;
using System.Collections.Generic;

namespace GestionIncidentes.Domain.Entities;

/// <summary>
/// Solución creada por un técnico para resolver un ticket.
/// Un ticket puede tener múltiples soluciones (diferentes intentos/enfoques).
/// Una solución puede promocionarse opcionalmente a la Base de Conocimiento.
/// </summary>
public class Solution
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Relación con Ticket
    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    
    // Información básica
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Técnico que creó la solución
    public Guid CreatedByUserId { get; set; }
    public User? CreatedBy { get; set; }
    
    // Estado de la solución
    public string Status { get; set; } = "En progreso"; // "En progreso", "Completada", "Fallida"
    public bool IsEffective { get; set; } = false; // ¿Resolvió el problema?
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    
    // Pasos de la solución
    public List<SolutionStep> Steps { get; set; } = new();
    
    // Relación con Knowledge Entry (si fue promocionada)
    public Guid? KnowledgeEntryId { get; set; }
    public KnowledgeEntry? KnowledgeEntry { get; set; }
    
    private Solution() { }

    public static Solution Create(
        Guid ticketId,
        string title,
        string description,
        Guid createdByUserId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título no puede estar vacío", nameof(title));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción no puede estar vacía", nameof(description));

        return new Solution
        {
            TicketId = ticketId,
            Title = title,
            Description = description,
            CreatedByUserId = createdByUserId,
            Status = "En progreso"
        };
    }

    public void MarkAsCompleted()
    {
        Status = "Completada";
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkAsEffective()
    {
        IsEffective = true;
        if (Status == "En progreso")
        {
            MarkAsCompleted();
        }
    }

    public void MarkAsFailed()
    {
        Status = "Fallida";
        CompletedAt = DateTime.UtcNow;
        IsEffective = false;
    }

    public void PromoteToKnowledgeBase(Guid knowledgeEntryId)
    {
        if (!IsEffective)
            throw new InvalidOperationException("Solo se pueden promocionar soluciones efectivas a la base de conocimiento");
        
        KnowledgeEntryId = knowledgeEntryId;
    }
}
