using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace GestionIncidentes.Infrastructure.Repositories
{
    public class EfUserRepository : IUserRepository
    {
        private readonly GestionIncidentesDbContext _context;

        public EfUserRepository(GestionIncidentesDbContext context)
        {
            _context = context;
        }

        // -------------------- Crear usuario --------------------
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == user.DepartmentId, ct);

            if (department == null)
                throw new InvalidOperationException("No se puede crear el usuario: el departamento no existe.");

            // Hashear la contraseña antes de guardar
            user.SetPassword(BCrypt.Net.BCrypt.HashPassword(user.Password));

            await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);
        }

        // -------------------- Obtener usuario por Id --------------------
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        // -------------------- Listar todos los usuarios --------------------
        public async Task<IEnumerable<User>> ListAllAsync(CancellationToken ct = default)
        {
            return await _context.Users.ToListAsync(ct);
        }

        // -------------------- Listar usuarios por departamento --------------------
        public async Task<IEnumerable<User>> ListByDepartmentAsync(Guid deptId, CancellationToken ct = default)
        {
            return await _context.Users
                .Where(u => u.DepartmentId == deptId)
                .ToListAsync(ct);
        }

        // -------------------- Actualizar usuario --------------------
        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == user.DepartmentId, ct);

            if (department == null)
                throw new InvalidOperationException("No se puede actualizar el usuario: el departamento no existe.");

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id, ct);

            if (existing == null)
                throw new InvalidOperationException("Usuario no encontrado para actualizar.");

            // Actualizar propiedades
            existing.SetFullName(user.FullName);
            existing.SetRole(user.Role);
            existing.SetDepartment(user.DepartmentId);

            // Hashear la contraseña si viene
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existing.SetPassword(BCrypt.Net.BCrypt.HashPassword(user.Password));
            }

            _context.Users.Update(existing);
            await _context.SaveChangesAsync(ct);
        }

        // -------------------- Eliminar usuario --------------------
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado para eliminar.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
    }
}
