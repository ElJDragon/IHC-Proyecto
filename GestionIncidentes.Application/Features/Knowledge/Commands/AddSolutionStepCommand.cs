using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge.Commands;

public record AddSolutionStepCommand(
    Guid KnowledgeEntryId,
    int StepNumber,
    string Title,
    string Description,
    string? ImageUrl = null
) : IRequest<SolutionStep>;
