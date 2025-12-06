using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Commands;

public record ReportIncidentCommand(string Title, string Description) : IRequest<Guid>;
