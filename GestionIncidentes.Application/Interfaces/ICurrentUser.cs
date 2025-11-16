using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionIncidentes.Application.Interfaces;
public interface ICurrentUser
{
    string? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    string DepartmentId { get; }
    // ← ESTE ES EL NUEVO
    int? RoleLevel { get; }
}


