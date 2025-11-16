using System;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Buscar el usuario
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado para eliminar.");

            // Eliminar del repositorio
            await _userRepository.DeleteAsync(request.Id, cancellationToken);

            return Unit.Value;
        }

        Task IRequestHandler<DeleteUserCommand>.Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }

    }
}
