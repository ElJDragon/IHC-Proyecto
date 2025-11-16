using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionIncidentes.Application.Interfaces;

    public interface IWorkloadService
    {
        Task<int> GetUserWorkloadAsync(Guid userId);
        Task RecalculateWorkloadAsync(Guid userId);
        Task<bool> CanAssignTicketAsync(Guid userId);
    }


