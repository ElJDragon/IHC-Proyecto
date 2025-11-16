using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Users.GetUser;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Models;
using MediatR;

namespace GestionIncidentes.Application.Features.Users.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                DepartmentId = user.DepartmentId,
                Workload = user.Workload
            };
        }
    }
}
