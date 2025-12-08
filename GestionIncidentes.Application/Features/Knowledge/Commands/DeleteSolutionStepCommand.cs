using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge.Commands;

public record DeleteSolutionStepCommand(Guid StepId) : IRequest;
