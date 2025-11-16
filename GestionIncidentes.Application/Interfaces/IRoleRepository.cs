using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Application.Interfaces;
namespace GestionIncidentes.Application.Interfaces;


    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid id);
        Task<List<Role>> ListAsync();
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Guid id);
    }
