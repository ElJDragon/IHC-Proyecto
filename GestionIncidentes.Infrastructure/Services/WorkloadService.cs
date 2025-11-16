using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionIncidentes.Application.Interfaces;
namespace GestionIncidentes.Infrastructure.Services;
public class WorkloadService : IWorkloadService
{
    private readonly IUserRepository _userRepo;
    private readonly ITicketRepository _ticketRepo;

    public WorkloadService(IUserRepository userRepo, ITicketRepository ticketRepo)
    {
        _userRepo = userRepo;
        _ticketRepo = ticketRepo;
    }

    public async Task<int> GetUserWorkloadAsync(Guid userId)
    {
        var tickets = await _ticketRepo.ListByUserAsync(userId);
        return tickets.Count();
    }

    public async Task RecalculateWorkloadAsync(Guid userId)
    {
        var workload = await GetUserWorkloadAsync(userId);
        var user = await _userRepo.GetByIdAsync(userId);

        if (user == null)
            return;

        user.SetWorkload (workload);
        await _userRepo.UpdateAsync(user);
    }

    public async Task<bool> CanAssignTicketAsync(Guid userId)
    {
        var workload = await GetUserWorkloadAsync(userId);
        return workload < 5;  // regla simple (puedes cambiarla)
    }
}
