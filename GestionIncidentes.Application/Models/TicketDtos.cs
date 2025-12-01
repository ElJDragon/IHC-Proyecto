namespace GestionIncidentes.Application.Models;

// DTO para crear ticket (Admin - formulario completo)
public record CreateAdminTicketDto(
    string Title,
    string Description,
    string Category, // hardware, software, connectivity, security
    string Priority, // critical, high, medium, low
    string AffectedType, // user, classroom, building, campus
    string Location, // Lab 1, Lab 2, etc.
    string LocationDetail, // Edificio A, Piso 3
    Guid? AssignedToUserId, // Técnico asignado (opcional al crear)
    Guid CreatedByUserId // Admin que crea el ticket
);

// DTO para crear reporte de estudiante
public record CreateStudentReportDto(
    string Lab, // Laboratorio seleccionado
    string EquipmentId, // ID del equipo
    string ProblemType, // "hardware" o "software"
    string? ProgramName, // Solo si es software
    string? ErrorMessage, // Solo si es software
    string? AffectedParts, // Solo si es hardware: "Monitor,Teclado,Mouse"
    Guid CreatedByUserId // Estudiante que reporta
);

// DTO para actualizar ticket (Admin/Técnico)
public record UpdateTicketDto(
    string? Title,
    string? Description,
    string? Category,
    string? Priority,
    string? Status,
    Guid? AssignedToUserId,
    string? TechnicianNotes
);

// DTO para cambiar estado (Técnico)
public record ChangeTicketStatusDto(
    string NewStatus, // Pendiente, En proceso, Resuelto
    string? TechnicianNotes
);

// DTO para cerrar ticket (Técnico)
public record CloseTicketDto(
    string TechnicianNotes,
    int? Rating // Opcional: valoración del usuario
);

// DTO para respuesta de ticket (usado en listados)
public record TicketResponseDto(
    Guid Id,
    string TicketNumber, // TKT-204
    string Title,
    string Description,
    string Category,
    string Priority,
    string Status,
    string Location,
    string LocationDetail,
    string? ProblemType,
    string? ProgramName,
    string? ErrorMessage,
    string? AffectedParts,
    Guid CreatedByUserId,
    string CreatedByName, // Nombre del reportante
    Guid? AssignedToUserId,
    string? AssignedToName, // Nombre del técnico
    string? TechnicianNotes,
    DateTime CreatedAt,
    DateTime? ResolvedAt,
    DateTime? SlaDeadline,
    string SlaStatus, // "2h", "6h", "-", "Vencido"
    int? Rating
);

// DTO para estadísticas del dashboard (Admin)
public record AdminDashboardStatsDto(
    int TotalIncidents,
    double AvgResolutionTime, // En horas
    string CriticalLab,
    int CriticalLabCount,
    double DepartmentEfficiency, // Porcentaje SLA cumplido
    List<LabIncidentDto> LabIncidents
);

public record LabIncidentDto(
    string Location,
    int Count
);

// DTO para estadísticas del técnico
public record TechnicianStatsDto(
    int AssignedTickets,
    int PendingTickets,
    int InProgressTickets,
    int ResolvedTickets,
    double AverageResolutionTime
);

// DTO para filtros de búsqueda
public record TicketFilterDto(
    string? Status,
    string? Priority,
    string? Location,
    string? Category,
    Guid? AssignedToUserId,
    Guid? CreatedByUserId
);

// DTO para actualizar notas del técnico
public record UpdateTechnicianNotesDto
{
    public string TechnicianNotes { get; set; } = "";
}

// DTO para calificar ticket (Estudiante)
public record RateTicketDto(
    int Rating, // 1-5
    string? FeedbackComment
);

// DTO para estadísticas de reportes del estudiante
public record StudentReportStatsDto(
    int TotalReports,
    int PendingReports,
    int InProgressReports,
    int ResolvedReports,
    double AverageResolutionTime
);

// DTO para estadísticas generales de tickets
public record TicketStatsDto(
    int TotalTickets,
    int PendingTickets,
    int InProgressTickets,
    int ResolvedTickets,
    int CriticalTickets,
    int HighTickets,
    int MediumTickets,
    int LowTickets,
    double AverageResolutionTime,
    double SlaComplianceRate
);

// DTO para incidentes por ubicación
public record LocationIncidentDto(
    string Location,
    int Count,
    List<TicketResponseDto> Tickets
);
