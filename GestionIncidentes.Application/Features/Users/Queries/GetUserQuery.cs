using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Application.Models;
using MediatR;
namespace Application.Features.Users.GetUser;
public record GetUserQuery(Guid Id) : IRequest<UserDto>;


