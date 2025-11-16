using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Users.Queries
{
  
        public record ListUsersByDepartmentQuery(Guid DepartmentId) : IRequest<IEnumerable<User>>;

    
}
