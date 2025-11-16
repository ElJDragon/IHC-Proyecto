using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Application.Commands;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepo;
        public CreateUserCommandHandler(IUserRepository userRepo) => _userRepo = userRepo;

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var user = User.Create(request.Email,request.Password, request.FullName, request.DepartmentId, request.Role);
            await _userRepo.AddAsync(user, ct);
            return user.Id;
        }

    }

}
