namespace GestionIncidentes.Domain.Entities;

public class SolutionStep
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid KnowledgeEntryId { get; private set; }
    public int StepNumber { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation property
    public KnowledgeEntry? KnowledgeEntry { get; private set; }

    private SolutionStep() { } // EF Core

    private SolutionStep(
        Guid knowledgeEntryId,
        int stepNumber,
        string title,
        string description,
        string? imageUrl = null)
    {
        Id = Guid.NewGuid();
        KnowledgeEntryId = knowledgeEntryId;
        StepNumber = stepNumber;
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
    }

    public static SolutionStep Create(
        Guid knowledgeEntryId,
        int stepNumber,
        string title,
        string description,
        string? imageUrl = null)
    {
        if (stepNumber < 1)
            throw new ArgumentException("El número de paso debe ser mayor a 0", nameof(stepNumber));
        
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título no puede estar vacío", nameof(title));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción no puede estar vacía", nameof(description));

        return new SolutionStep(knowledgeEntryId, stepNumber, title, description, imageUrl);
    }

    public void Update(string title, string description, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título no puede estar vacío", nameof(title));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción no puede estar vacía", nameof(description));

        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStepNumber(int newStepNumber)
    {
        if (newStepNumber < 1)
            throw new ArgumentException("El número de paso debe ser mayor a 0", nameof(newStepNumber));
        
        StepNumber = newStepNumber;
        UpdatedAt = DateTime.UtcNow;
    }
}
