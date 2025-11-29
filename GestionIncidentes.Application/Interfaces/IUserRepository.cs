using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Domain.Entities;

namespace GestionIncidentes.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken ct = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<User>> ListAllAsync(CancellationToken ct = default);
        Task<List<User>> GetAllAsync(); // ✅ Agregado para los event handlers
        Task<IEnumerable<User>> ListByDepartmentAsync(Guid deptId, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
