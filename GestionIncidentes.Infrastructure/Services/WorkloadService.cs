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
        // Límite máximo de 3 tickets activos por técnico
        return workload < 3;
    }

    public async Task<(bool CanAssign, string? AlertMessage)> ValidateAssignmentAsync(Guid userId)
    {
        // Verificar límite de 3 tickets
        var currentWorkload = await GetUserWorkloadAsync(userId);
        if (currentWorkload >= 3)
        {
            return (false, "Este técnico ya tiene 3 incidentes asignados (límite máximo).");
        }

        // Verificar distribución equitativa (última semana)
        var allTechnicians = await _userRepo.GetAllAsync();
        var technicians = allTechnicians.Where(u => u.Role == "Technician").ToList();
        
        if (technicians.Count == 0)
            return (true, null);

        var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
        var workloads = new Dictionary<Guid, int>();

        foreach (var tech in technicians)
        {
            var tickets = await _ticketRepo.ListByUserAsync(tech.Id);
            var recentTickets = tickets.Count(t => t.CreatedAt >= oneWeekAgo);
            workloads[tech.Id] = recentTickets;
        }

        if (workloads.Count > 1)
        {
            var avgWorkload = workloads.Values.Average();
            var maxWorkload = workloads.Values.Max();
            var targetWorkload = workloads.GetValueOrDefault(userId, 0);

            // Alerta si este técnico tiene 2+ incidentes más que el promedio
            if (targetWorkload >= avgWorkload + 2)
            {
                return (false, $"⚠️ Este técnico ha recibido {targetWorkload} incidentes en la última semana, mientras el promedio es {avgWorkload:F1}. Se recomienda asignar a otro técnico para distribuir la carga equitativamente.");
            }
        }

        return (true, null);
    }
}
