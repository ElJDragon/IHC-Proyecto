using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Commands;

public record CreateIncidentCommand(
    string Title,
    string Description
) : IRequest<Guid>;
