using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge;

public record SearchKnowledgeQuery(string SearchTerm) : IRequest<List<KnowledgeEntryDto>>;

public class KnowledgeEntryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Problem { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int UsageCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
