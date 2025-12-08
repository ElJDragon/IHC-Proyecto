using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge.Queries;

public record GetSolutionStepsByKnowledgeEntryQuery(Guid KnowledgeEntryId) : IRequest<List<SolutionStep>>;
