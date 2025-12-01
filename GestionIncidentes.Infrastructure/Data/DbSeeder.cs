using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(GestionIncidentesDbContext context)
    {
        // Verificar si ya hay datos
        if (await context.Users.AnyAsync(u => u.Email == "admin@universidad.edu"))
        {
            Console.WriteLine("Los datos de prueba ya existen.");
            return;
        }

        Console.WriteLine("Iniciando carga de datos de prueba...");

        // 1. CREAR DEPARTAMENTO
        var department = Department.Create("Soporte Técnico");
        context.Departments.Add(department);
        await context.SaveChangesAsync();

        // 2. CREAR USUARIOS
        var admin = User.Create(
            "admin@universidad.edu",
            "Admin123!",
            "Carlos Rodríguez",
            department.Id,
            "Admin"
        );

        var tech1 = User.Create(
            "carlos.mendez@universidad.edu",
            "Tech123!",
            "Carlos Méndez",
            department.Id,
            "Technician"
        );

        var tech2 = User.Create(
            "ana.torres@universidad.edu",
            "Tech123!",
            "Ana Torres",
            department.Id,
            "Technician"
        );

        var tech3 = User.Create(
            "luis.garcia@universidad.edu",
            "Tech123!",
            "Luis García",
            department.Id,
            "Technician"
        );

        var student1 = User.Create(
            "juan.perez@universidad.edu",
            "Student123!",
            "Juan Pérez",
            department.Id, // Usar el mismo departamento
            "Student"
        );

        var student2 = User.Create(
            "maria.lopez@universidad.edu",
            "Student123!",
            "María López",
            department.Id,
            "Student"
        );

        var student3 = User.Create(
            "pedro.gonzalez@universidad.edu",
            "Student123!",
            "Pedro González",
            department.Id,
            "Student"
        );

        var users = new[] { admin, tech1, tech2, tech3, student1, student2, student3 };
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        var adminId = admin.Id;
        var tech1Id = tech1.Id;
        var tech2Id = tech2.Id;
        var tech3Id = tech3.Id;
        var student1Id = student1.Id;
        var student2Id = student2.Id;
        var student3Id = student3.Id;

        // 3. CREAR TICKETS DE PRUEBA
        var tickets = new List<Ticket>
        {
            // Ticket 1: Problema crítico de conectividad
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "[SEED] Router sin respuesta",
                Description = "El router principal del laboratorio no responde a pings. Los estudiantes no pueden acceder a internet.",
                Category = "connectivity",
                Priority = "critical",
                Status = "En proceso",
                Location = "Lab 1",
                AffectedType = "classroom",
                CreatedByUserId = student1Id,
                AssignedToUserId = tech1Id,
                CreatedAt = DateTime.UtcNow.AddHours(-2),
                SlaDeadline = DateTime.UtcNow.AddHours(2)
            },
            // Ticket 2: Problema de hardware resuelto
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "[SEED] Monitor sin señal",
                Description = "El monitor de la estación 15 no muestra imagen.",
                Category = "hardware",
                Priority = "medium",
                Status = "Resuelto",
                Location = "Lab 3",
                AffectedType = "classroom",
                ProblemType = "hardware",
                AffectedParts = "Monitor",
                CreatedByUserId = student1Id,
                AssignedToUserId = tech2Id,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ResolvedAt = DateTime.UtcNow.AddHours(-12),
                SlaDeadline = DateTime.UtcNow.AddHours(-12),
                TechnicianNotes = "Se reemplazó el cable HDMI que estaba dañado.",
                Rating = 5,
                FeedbackComment = "Excelente servicio, muy rápido."
            },
            // Ticket 3: Problema de software pendiente
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "[SEED] Error en Visual Studio",
                Description = "Visual Studio no compila proyectos y muestra error de MSBuild.",
                Category = "software",
                Priority = "high",
                Status = "Pendiente",
                Location = "Lab 2",
                AffectedType = "classroom",
                ProblemType = "software",
                ProgramName = "Visual Studio 2022",
                ErrorMessage = "MSBuild error: Cannot find SDK",
                CreatedByUserId = student2Id,
                AssignedToUserId = tech3Id,
                CreatedAt = DateTime.UtcNow.AddHours(-3),
                SlaDeadline = DateTime.UtcNow.AddHours(6)
            },
            // Ticket 4: Problema de hardware en proceso
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "[SEED] Teclado no responde",
                Description = "El teclado de la PC 08 no funciona correctamente.",
                Category = "hardware",
                Priority = "low",
                Status = "En proceso",
                Location = "Lab 1",
                AffectedType = "classroom",
                ProblemType = "hardware",
                EquipmentId = "PC-08",
                AffectedParts = "Teclado",
                CreatedByUserId = student2Id,
                AssignedToUserId = tech1Id,
                CreatedAt = DateTime.UtcNow.AddHours(-5),
                SlaDeadline = DateTime.UtcNow.AddHours(24),
                TechnicianNotes = "Se solicitó un teclado nuevo al almacén."
            },
            // Ticket 5: Problema crítico de WiFi
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "[SEED] WiFi no disponible",
                Description = "La red WiFi \"Universidad_Estudiantes\" no aparece en el área de la biblioteca.",
                Category = "connectivity",
                Priority = "critical",
                Status = "Pendiente",
                Location = "Biblioteca",
                LocationDetail = "Segundo piso",
                AffectedType = "library",
                CreatedByUserId = student3Id,
                AssignedToUserId = tech2Id,
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                SlaDeadline = DateTime.UtcNow.AddHours(2)
            },
            // Ticket 6: Problema de hardware resuelto con valoración
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "[SEED] Monitor parpadeando",
                Description = "El monitor parpadea constantemente.",
                Category = "hardware",
                Priority = "medium",
                Status = "Resuelto",
                Location = "Lab 4",
                AffectedType = "classroom",
                ProblemType = "hardware",
                AffectedParts = "Monitor",
                CreatedByUserId = student3Id,
                AssignedToUserId = tech3Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                ResolvedAt = DateTime.UtcNow.AddDays(-1),
                SlaDeadline = DateTime.UtcNow.AddDays(-2).AddHours(12),
                TechnicianNotes = "Se ajustó la frecuencia de refresco a 60Hz.",
                Rating = 4,
                FeedbackComment = "Bien resuelto."
            },
            // Ticket 7: Problema de software resuelto
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "[SEED] Licencia de AutoCAD vencida",
                Description = "AutoCAD muestra mensaje de licencia vencida en todas las PCs.",
                Category = "software",
                Priority = "high",
                Status = "Resuelto",
                Location = "Lab 3",
                AffectedType = "classroom",
                ProblemType = "software",
                ProgramName = "AutoCAD 2024",
                CreatedByUserId = student1Id,
                AssignedToUserId = tech1Id,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                ResolvedAt = DateTime.UtcNow.AddDays(-2),
                SlaDeadline = DateTime.UtcNow.AddDays(-3).AddHours(6),
                TechnicianNotes = "Se renovó la licencia educativa con Autodesk.",
                Rating = 5
            },
            // Ticket 8: Problema de hardware en proceso
            new Ticket
            {
                Id = Guid.NewGuid(),
                Title = "[SEED] Proyector no enciende",
                Description = "El proyector del auditorio no responde al control remoto ni al botón de encendido.",
                Category = "hardware",
                Priority = "medium",
                Status = "En proceso",
                Location = "Auditorio",
                AffectedType = "auditorium",
                ProblemType = "hardware",
                AffectedParts = "Proyector",
                CreatedByUserId = adminId,
                AssignedToUserId = tech2Id,
                CreatedAt = DateTime.UtcNow.AddHours(-4),
                SlaDeadline = DateTime.UtcNow.AddHours(12),
                TechnicianNotes = "Se está verificando la fuente de poder. Posible reemplazo necesario."
            }
        };

        context.Tickets.AddRange(tickets);
        await context.SaveChangesAsync();

        Console.WriteLine("========================================");
        Console.WriteLine("✅ Datos de prueba cargados exitosamente");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("CREDENCIALES DE ACCESO:");
        Console.WriteLine();
        Console.WriteLine("Admin:");
        Console.WriteLine("  Email: admin@universidad.edu");
        Console.WriteLine("  Password: Admin123!");
        Console.WriteLine();
        Console.WriteLine("Técnicos:");
        Console.WriteLine("  carlos.mendez@universidad.edu / Tech123!");
        Console.WriteLine("  ana.torres@universidad.edu / Tech123!");
        Console.WriteLine("  luis.garcia@universidad.edu / Tech123!");
        Console.WriteLine();
        Console.WriteLine("Estudiantes:");
        Console.WriteLine("  juan.perez@universidad.edu / Student123!");
        Console.WriteLine("  maria.lopez@universidad.edu / Student123!");
        Console.WriteLine("  pedro.gonzalez@universidad.edu / Student123!");
        Console.WriteLine();
    }
}
