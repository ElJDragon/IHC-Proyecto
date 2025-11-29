using System;

namespace GestionIncidentes.Domain.Entities;

/// <summary>
/// Reporte de ticket con información de resolución
/// </summary>
public class TicketReport
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TicketId { get; set; }
    public Guid TechnicianId { get; set; }
    public string ResolutionDetails { get; set; } = string.Empty;
    public string ProblemDiagnosis { get; set; } = string.Empty;
    public string ActionsTaken { get; set; } = string.Empty;
    public TimeSpan TimeSpent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool SuggestKnowledgeEntry { get; set; } = false; // Si el técnico sugiere agregar a KDB

    private TicketReport() { }

    public static TicketReport Create(
        Guid ticketId,
        Guid technicianId,
        string resolutionDetails,
        string problemDiagnosis,
        string actionsTaken,
        TimeSpan timeSpent,
        bool suggestKnowledgeEntry = false)
    {
        return new TicketReport
        {
            TicketId = ticketId,
            TechnicianId = technicianId,
            ResolutionDetails = resolutionDetails,
            ProblemDiagnosis = problemDiagnosis,
            ActionsTaken = actionsTaken,
            TimeSpent = timeSpent,
            SuggestKnowledgeEntry = suggestKnowledgeEntry
        };
    }
}
