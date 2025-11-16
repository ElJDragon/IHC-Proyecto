using System;
using MediatR;

namespace GestionIncidentes.Application.Commands
{
    // Comando para actualizar un usuario
    public record UpdateUserCommand(
        Guid Id,
        string FullName,
        Guid DepartmentId,
        string Role
    ) : IRequest;
}
