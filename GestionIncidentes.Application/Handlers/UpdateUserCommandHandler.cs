using System;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository, IDepartmentRepository departmentRepository)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Buscar el usuario
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado para actualizar.");

            // Validar que el departamento exista
            var dept = await _departmentRepository.GetByIdAsync(request.DepartmentId);
            if (dept == null)
                throw new InvalidOperationException("El departamento no existe.");

            // Actualizar usuario usando métodos de dominio
            user.SetFullName(request.FullName);
            user.SetDepartment(request.DepartmentId);
            user.SetRole(request.Role);

            // Guardar cambios
            await _userRepository.UpdateAsync(user, cancellationToken);

            return Unit.Value;
        }

        Task IRequestHandler<UpdateUserCommand>.Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return Handle(request, cancellationToken);
        }
    }
}
