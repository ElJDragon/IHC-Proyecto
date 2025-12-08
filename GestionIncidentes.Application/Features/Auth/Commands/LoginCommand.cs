using MediatR;

namespace GestionIncidentes.Application.Features.Auth.Commands;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResult>;

public record LoginResult(
    bool Success,
    string? Message,
    Guid? UserId,
    string? UserName,
    string? Role
);
