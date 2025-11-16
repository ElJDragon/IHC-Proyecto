using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Application.Interfaces;

namespace GestionIncidentes.Infrastructure.Repositories
{
    public class InMemoryDepartmentRepository : IDepartmentRepository
    {
        private readonly List<Department> _departments = new();

        // Crear departamento
        public Task AddAsync(Department dept)
        {
            _departments.Add(dept);
            return Task.CompletedTask;
        }

        // Obtener departamento por Id
        public Task<Department?> GetByIdAsync(Guid id)
        {
            var dept = _departments.FirstOrDefault(d => d.Id == id);
            return Task.FromResult(dept);
        }

        // Listar todos los departamentos
        public Task<IEnumerable<Department>> ListAsync()
        {
            return Task.FromResult<IEnumerable<Department>>(_departments);
        }

        // Actualizar departamento
        public Task UpdateAsync(Department dept)
        {
            var index = _departments.FindIndex(d => d.Id == dept.Id);
            if (index >= 0)
            {
                _departments[index] = dept;
            }
            return Task.CompletedTask;
        }

        // Eliminar departamento
        public Task DeleteAsync(Guid id)
        {
            var dept = _departments.FirstOrDefault(d => d.Id == id);
            if (dept != null)
            {
                _departments.Remove(dept);
            }
            return Task.CompletedTask;
        }
    }
}
