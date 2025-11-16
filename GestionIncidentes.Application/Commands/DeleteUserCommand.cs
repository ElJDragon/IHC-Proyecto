using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionIncidentes.Application.Commands;
using System;
using MediatR;


    // Comando para eliminar un usuario por ID
    public record DeleteUserCommand(Guid Id) : IRequest;

