using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure.Repositories
{

    public class EfDepartmentRepository : IDepartmentRepository
    {
        private readonly GestionIncidentesDbContext _context;

        public EfDepartmentRepository(GestionIncidentesDbContext context)
        {
            _context = context;
        }

        // Crear departamento
        public async Task AddAsync(Department dept)
        {
            await _context.Departments.AddAsync(dept);
            await _context.SaveChangesAsync();
        }

        // Obtener departamento por Id
        public async Task<Department?> GetByIdAsync(Guid id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        // Listar todos los departamentos
        public async Task<IEnumerable<Department>> ListAsync()
        {
            return await _context.Departments
                .Include(d => d.Users) // ✅ ahora trae también los usuarios
                .ToListAsync();
        }
        // Actualizar departamento
        public async Task UpdateAsync(Department dept)
        {
            var existing = await _context.Departments.FirstOrDefaultAsync(d => d.Id == dept.Id);
            if (existing != null)
            {
                existing.Name = dept.Name;
                // Aquí puedes actualizar más propiedades si las hay
                _context.Departments.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        // Eliminar departamento
        public async Task DeleteAsync(Guid id)
        {
            var dept = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            Console.WriteLine($"ID recibido: {id}");
            if (dept == null)
            {
                Console.WriteLine("No se encontró el departamento en la base");
                return;
            }

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Departamento eliminado: {dept.Name}");
        }

    }
}
