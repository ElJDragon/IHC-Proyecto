using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Domain.Entities;
namespace GestionIncidentes.Application.Interfaces;
public interface IDepartmentRepository
{
    Task AddAsync(Department department);
    Task<Department?> GetByIdAsync(Guid id);
    Task<IEnumerable<Department>> ListAsync();
    Task UpdateAsync(Department department);
    Task DeleteAsync(Guid id);
 
}
