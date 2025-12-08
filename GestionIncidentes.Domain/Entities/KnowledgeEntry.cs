using System;

namespace GestionIncidentes.Domain.Entities;

/// <summary>
/// Entrada en la Base de Conocimiento (Knowledge Database)
/// </summary>
public class KnowledgeEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Problem { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Software, Hardware, Red, etc.
    public List<string> Tags { get; set; } = new();
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int UsageCount { get; set; } = 0; // Cu�ntas veces se ha consultado
    public bool IsPublished { get; set; } = true;

    // Relación con tickets resueltos
    public Guid? RelatedTicketId { get; set; }

    // Pasos de la solución
    public List<SolutionStep> Steps { get; set; } = new();

    private KnowledgeEntry() { }

    public static KnowledgeEntry Create(
        string title,
        string problem,
        string solution,
        string category,
        Guid createdByUserId,
        Guid? relatedTicketId = null,
        List<string>? tags = null)
    {
        return new KnowledgeEntry
        {
            Title = title,
            Problem = problem,
            Solution = solution,
            Category = category,
            CreatedByUserId = createdByUserId,
            RelatedTicketId = relatedTicketId,
            Tags = tags ?? new List<string>()
        };
    }

    public void Update(string title, string problem, string solution, string category, List<string>? tags = null)
    {
        Title = title;
        Problem = problem;
        Solution = solution;
        Category = category;
        if (tags != null) Tags = tags;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementUsage()
    {
        UsageCount++;
    }
}
