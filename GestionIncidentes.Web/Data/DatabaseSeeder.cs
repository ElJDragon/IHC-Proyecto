using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Web.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(GestionIncidentesDbContext context)
    {
        // Asegurar que la base de datos existe
        await context.Database.MigrateAsync();

        // Verificar si los usuarios de prueba existen
        var testUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var testUserExists = await context.Users.AnyAsync(u => u.Id == testUserId);

        if (testUserExists)
        {
            Console.WriteLine("??  Los usuarios de prueba ya existen en la base de datos");
            return;
        }

        Console.WriteLine("?? Creando usuarios de prueba...");

        // Crear departamento de prueba
        var departmentId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        // Usar SQL crudo para insertar datos con IDs específicos
        await context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO ""Departments"" (""Id"", ""Name"")
            VALUES ('11111111-1111-1111-1111-111111111111', 'Departamento TI')
            ON CONFLICT (""Id"") DO NOTHING;
        ");

        await context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO ""Users"" (""Id"", ""Email"", ""Password"", ""FullName"", ""DepartmentId"", ""Role"", ""Workload"")
            VALUES 
                ('00000000-0000-0000-0000-000000000001', 'admin@test.com', 'password123', 'Administrador Sistema', '11111111-1111-1111-1111-111111111111', 'DITIC', 0),
                ('00000000-0000-0000-0000-000000000002', 'tecnico@test.com', 'password123', 'Técnico de Soporte', '11111111-1111-1111-1111-111111111111', 'Tecnico', 0),
                ('00000000-0000-0000-0000-000000000003', 'estudiante@test.com', 'password123', 'Estudiante de Prueba', '11111111-1111-1111-1111-111111111111', 'Estudiante', 0)
            ON CONFLICT (""Id"") DO UPDATE SET
                ""Email"" = EXCLUDED.""Email"",
                ""Password"" = EXCLUDED.""Password"",
                ""FullName"" = EXCLUDED.""FullName"",
                ""DepartmentId"" = EXCLUDED.""DepartmentId"",
                ""Role"" = EXCLUDED.""Role"",
                ""Workload"" = EXCLUDED.""Workload"";
        ");

        Console.WriteLine("? Usuarios de prueba creados exitosamente");
        Console.WriteLine("   ?? Usuarios disponibles:");
        Console.WriteLine("   • admin@test.com (DITIC) - ID: 00000000-0000-0000-0000-000000000001");
        Console.WriteLine("   • tecnico@test.com (Tecnico) - ID: 00000000-0000-0000-0000-000000000002");
        Console.WriteLine("   • estudiante@test.com (Estudiante) - ID: 00000000-0000-0000-0000-000000000003");
    }
}
