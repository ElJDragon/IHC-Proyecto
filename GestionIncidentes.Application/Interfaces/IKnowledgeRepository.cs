using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces;

public interface IKnowledgeRepository
{
    Task<KnowledgeEntry?> GetByIdAsync(Guid id);
    Task<List<KnowledgeEntry>> SearchAsync(string searchTerm);
    Task<List<KnowledgeEntry>> GetByCategoryAsync(string category);
    Task<List<KnowledgeEntry>> GetByTagsAsync(List<string> tags);
    Task<List<KnowledgeEntry>> GetAllAsync();
    Task AddAsync(KnowledgeEntry entry);
    Task UpdateAsync(KnowledgeEntry entry);
    Task DeleteAsync(Guid id);
}
