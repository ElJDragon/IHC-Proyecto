using System;

namespace GestionIncidentes.Application.Models
{
    // DTO para crear ticket desde admin o estudiante
    public record CreateTicketDto(
        string Title,
        string Description,
        string Category,
        string Priority,
        string Location,
        string? LocationDetail,
        string AffectedType,
        string? ProblemType,
        string? ProgramName,
        string? ErrorMessage,
        string? AffectedParts,
        string? EquipmentId,
        Guid CreatedByUserId,
        Guid? AssignedToUserId
    );
}
