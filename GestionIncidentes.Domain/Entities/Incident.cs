namespace GestionIncidentes.Domain.Entities;

public class Incident
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid ReportedByUserId { get; private set; }
    public DateTime ReportedAt { get; private set; }
    public string Status { get; private set; } = "Reported"; // Reported, InProgress, Resolved
    public Guid? AssignedTicketId { get; private set; }

    private Incident() { } // Para EF

    public static Incident Create(string title, string description, Guid reportedByUserId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título es requerido", nameof(title));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción es requerida", nameof(description));

        return new Incident
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            ReportedByUserId = reportedByUserId,
            ReportedAt = DateTime.UtcNow,
            Status = "Reported"
        };
    }

    public void AssignTicket(Guid ticketId)
    {
        AssignedTicketId = ticketId;
        Status = "InProgress";
    }

    public void Resolve()
    {
        Status = "Resolved";
    }

    public void UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("El estado es requerido", nameof(status));
        
        Status = status;
    }
}
