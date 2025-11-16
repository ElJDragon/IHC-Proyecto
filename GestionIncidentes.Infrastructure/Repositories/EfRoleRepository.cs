using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure.Repositories
{
    public class EfRoleRepository : IRoleRepository
    {
        private readonly GestionIncidentesDbContext _context;

        public EfRoleRepository(GestionIncidentesDbContext context)
        {
            _context = context;
        }

        // Obtener rol por Id
        public async Task<Role?> GetByIdAsync(Guid id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        // Listar todos los roles
        public async Task<List<Role>> ListAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        // Crear rol
        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        // Actualizar rol
        public async Task UpdateAsync(Role role)
        {
            var existing = await _context.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);
            if (existing != null)
            {
                existing.Name = role.Name;
                existing.Level = role.Level;
                _context.Roles.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        // Eliminar rol
        public async Task DeleteAsync(Guid id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
