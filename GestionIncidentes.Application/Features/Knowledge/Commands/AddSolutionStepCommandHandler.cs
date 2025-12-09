using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge.Commands;

public class AddSolutionStepCommandHandler : IRequestHandler<AddSolutionStepCommand, SolutionStep>
{
    private readonly ISolutionStepRepository _repository;

    public AddSolutionStepCommandHandler(ISolutionStepRepository repository)
    {
        _repository = repository;
    }

    public async Task<SolutionStep> Handle(AddSolutionStepCommand request, CancellationToken cancellationToken)
    {
        var step = SolutionStep.Create(
            request.KnowledgeEntryId,
            request.StepNumber,
            request.Title,
            request.Description,
            request.ImageUrl
        );

        return await _repository.CreateAsync(step);
    }
}
