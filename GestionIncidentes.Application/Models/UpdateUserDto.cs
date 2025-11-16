using System;

namespace GestionIncidentes.Application.Models
{
    public class UpdateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }

        // Rol único
        public string Role { get; set; } = string.Empty;
    }
}

