namespace GestionIncidentes.Domain.Entities;

public class SolutionStep
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SolutionId { get; private set; }
    public int StepNumber { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation property
    public Solution? Solution { get; private set; }

    private SolutionStep() { } // EF Core

    private SolutionStep(
        Guid solutionId,
        int stepNumber,
        string title,
        string description,
        string? imageUrl = null)
    {
        Id = Guid.NewGuid();
        SolutionId = solutionId;
        StepNumber = stepNumber;
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
    }

    public static SolutionStep Create(
        Guid solutionId,
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

        return new SolutionStep(solutionId, stepNumber, title, description, imageUrl);
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
