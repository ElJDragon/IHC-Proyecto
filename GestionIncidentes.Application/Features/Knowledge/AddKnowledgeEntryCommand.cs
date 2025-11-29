using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge;

public record AddKnowledgeEntryCommand(
    string Title,
    string Problem,
    string Solution,
    string Category,
    Guid CreatedByUserId,
    Guid? RelatedTicketId = null,
    List<string>? Tags = null
) : IRequest<Guid>;
