using System;
using MediatR;

namespace GestionIncidentes.Application.Commands
{
    public record CreateUserCommand(string Email, string Password, string FullName, Guid DepartmentId, string Role) : IRequest<Guid>;
}
