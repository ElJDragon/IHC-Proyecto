using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _store = new();
        private readonly IDepartmentRepository _departmentRepository;

        public InMemoryUserRepository(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // -------------------- Crear usuario --------------------
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            var department = await _departmentRepository.GetByIdAsync(user.DepartmentId);
            if (department == null)
                throw new InvalidOperationException("No se puede crear el usuario: el departamento no existe.");

            _store.Add(user);
        }

        // -------------------- Obtener usuario por Id --------------------
        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => Task.FromResult(_store.FirstOrDefault(u => u.Id == id));

        // -------------------- Listar todos los usuarios --------------------
        public Task<IEnumerable<User>> ListAllAsync(CancellationToken ct = default)
            => Task.FromResult<IEnumerable<User>>(_store);

        // ✅ Implementación de GetAllAsync para event handlers
        public Task<List<User>> GetAllAsync()
            => Task.FromResult(_store.ToList());

        // -------------------- Listar usuarios por departamento --------------------
        public Task<IEnumerable<User>> ListByDepartmentAsync(Guid deptId, CancellationToken ct = default)
            => Task.FromResult<IEnumerable<User>>(_store.Where(u => u.DepartmentId == deptId));

        // -------------------- Actualizar usuario --------------------
        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            var department = await _departmentRepository.GetByIdAsync(user.DepartmentId);
            if (department == null)
                throw new InvalidOperationException("No se puede actualizar el usuario: el departamento no existe.");

            var idx = _store.FindIndex(u => u.Id == user.Id);
            if (idx >= 0)
                _store[idx] = user;
            else
                throw new InvalidOperationException("Usuario no encontrado para actualizar.");
        }

        // -------------------- Eliminar usuario --------------------
        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var user = _store.FirstOrDefault(u => u.Id == id);
            if (user != null)
                _store.Remove(user);
            else
                throw new InvalidOperationException("Usuario no encontrado para eliminar.");

            return Task.CompletedTask;
        }
    }
}
