using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionIncidentes.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Usuarios relacionados
    public Guid CreatedByUserId { get; set; } // Usuario que reportó (estudiante)
    public Guid? AssignedToUserId { get; set; } // Técnico asignado (nullable)

    // Campos de categorización
    public string Category { get; set; } = string.Empty; // hardware, software, connectivity, security
    public string Priority { get; set; } = "medium"; // critical, high, medium, low
    public string Status { get; set; } = "Pendiente"; // Pendiente, En proceso, Resuelto
    
    // Ubicación y afectación
    public string Location { get; set; } = string.Empty; // Lab 1, Lab 2, Biblioteca, etc.
    public string AffectedType { get; set; } = string.Empty; // user, classroom, building, campus
    public string LocationDetail { get; set; } = string.Empty; // Edificio A, Piso 3
    
    // Campos específicos para estudiantes
    public string? ProblemType { get; set; } // "hardware" o "software"
    public string? EquipmentId { get; set; } // ID del equipo reportado
    public string? ProgramName { get; set; } // Para problemas de software
    public string? ErrorMessage { get; set; } // Mensaje de error del software
    public string? AffectedParts { get; set; } // Lista separada por comas: "Monitor,Teclado,Mouse"
    
    // Gestión y resolución
    public string? TechnicianNotes { get; set; } // Bitácora de solución
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }
    public DateTime? SlaDeadline { get; set; } // Fecha límite según SLA
    
    // Valoración
    public int? Rating { get; set; } // 1-5 estrellas
    public string? FeedbackComment { get; set; }
}
