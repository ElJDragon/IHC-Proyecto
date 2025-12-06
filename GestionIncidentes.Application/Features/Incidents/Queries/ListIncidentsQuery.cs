using GestionIncidentes.Application.Features.Incidents.Dtos;
using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Queries;

public record ListIncidentsQuery : IRequest<List<IncidentDto>>;
