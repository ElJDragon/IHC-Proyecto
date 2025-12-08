using MediatR;

namespace GestionIncidentes.Application.Features.Incidents.Commands;

/// <summary>
/// Comando para asignar un incidente a un técnico, creando un ticket
/// </summary>
public record AssignIncidentToTechnicianCommand(
    Guid IncidentId,
    Guid TechnicianId
) : IRequest<Guid>; // Retorna el ID del Ticket creado
