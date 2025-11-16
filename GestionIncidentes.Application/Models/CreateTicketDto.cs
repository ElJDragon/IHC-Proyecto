using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionIncidentes.Application.Models
{
    public record CreateTicketDto
 (
     string Title,
     string Description,
     Guid UserId,
     Guid CreatedByUserId
 );

}
