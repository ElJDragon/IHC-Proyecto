using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge;

public class AddKnowledgeEntryCommandHandler : IRequestHandler<AddKnowledgeEntryCommand, Guid>
{
    private readonly IKnowledgeRepository _knowledgeRepository;
    private readonly IAuditLogRepository _auditLogRepository;

    public AddKnowledgeEntryCommandHandler(
        IKnowledgeRepository knowledgeRepository,
        IAuditLogRepository auditLogRepository)
    {
        _knowledgeRepository = knowledgeRepository;
        _auditLogRepository = auditLogRepository;
    }

    public async Task<Guid> Handle(AddKnowledgeEntryCommand request, CancellationToken cancellationToken)
    {
        var entry = KnowledgeEntry.Create(
            request.Title,
            request.Problem,
            request.Solution,
            request.Category,
            request.CreatedByUserId,
            request.RelatedTicketId,
            request.Tags
        );

        await _knowledgeRepository.AddAsync(entry);

        // Auditoría
        var auditLog = AuditLog.Create(
            request.CreatedByUserId,
            "AddedKnowledgeEntry",
            "KnowledgeEntry",
            entry.Id,
            $"Title: {request.Title}, Category: {request.Category}"
        );
        await _auditLogRepository.AddAsync(auditLog);

        return entry.Id;
    }
}
