using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Application.Models;
using MediatR;

namespace GestionIncidentes.Application.Features.Users.Queries;
public record ListUsersQuery() : IRequest<IEnumerable<UserDto>>;


