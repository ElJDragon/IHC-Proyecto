using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Knowledge.Commands;

public class DeleteSolutionStepCommandHandler : IRequestHandler<DeleteSolutionStepCommand>
{
    private readonly ISolutionStepRepository _repository;

    public DeleteSolutionStepCommandHandler(ISolutionStepRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteSolutionStepCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.StepId);
    }
}
