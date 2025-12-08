using MediatR;
using GestionIncidentes.Application.Interfaces;

namespace GestionIncidentes.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepo;

    public LoginCommandHandler(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Buscar usuario por email
        var users = await _userRepo.ListAllAsync();
        var user = users.FirstOrDefault(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            return new LoginResult(
                Success: false,
                Message: "Credenciales incorrectas",
                UserId: null,
                UserName: null,
                Role: null
            );
        }

        // Verificar contraseña (en producción usar hashing)
        if (user.Password != request.Password)
        {
            return new LoginResult(
                Success: false,
                Message: "Credenciales incorrectas",
                UserId: null,
                UserName: null,
                Role: null
            );
        }

        return new LoginResult(
            Success: true,
            Message: "Login exitoso",
            UserId: user.Id,
            UserName: user.FullName,
            Role: user.Role
        );
    }
}
