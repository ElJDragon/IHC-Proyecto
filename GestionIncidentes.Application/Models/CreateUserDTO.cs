using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionIncidentes.Application.Models;
public class CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    
    public string Password { get; set; } = string.Empty;  // ✅ nueva
      

    // NUEVO: rol único
    public string Role { get; set; } = string.Empty;
}
