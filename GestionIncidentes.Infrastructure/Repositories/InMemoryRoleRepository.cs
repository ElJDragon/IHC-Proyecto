using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class InMemoryRoleRepository : IRoleRepository
    {
        private readonly List<Role> _roles = new();

        public Task<Role?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_roles.FirstOrDefault(r => r.Id == id));
        }

        public Task<List<Role>> ListAsync()
        {
            return Task.FromResult(_roles.ToList());
        }

        public Task AddAsync(Role role)
        {
            _roles.Add(role);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Role role)
        {
            var existing = _roles.FirstOrDefault(r => r.Id == role.Id);
            if (existing != null)
            {
                existing.Name = role.Name;
                existing.Level = role.Level;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var role = _roles.FirstOrDefault(r => r.Id == id);
            if (role != null)
                _roles.Remove(role);

            return Task.CompletedTask;
        }
    }
}
