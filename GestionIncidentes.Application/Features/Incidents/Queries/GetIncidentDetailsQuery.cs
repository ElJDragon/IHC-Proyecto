using GestionIncidentes.Application.Features.Incidents.Dtos;
using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Queries;

public record GetIncidentDetailsQuery(Guid IncidentId) : IRequest<IncidentDto?>;
