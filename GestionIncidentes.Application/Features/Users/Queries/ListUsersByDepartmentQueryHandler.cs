using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Users.Queries
{
    public class ListUsersByDepartmentQueryHandler : IRequestHandler<ListUsersByDepartmentQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _userRepository;

        public ListUsersByDepartmentQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> Handle(ListUsersByDepartmentQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.ListByDepartmentAsync(request.DepartmentId, cancellationToken);
            return users;
        }
    }
}
