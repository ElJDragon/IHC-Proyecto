using GestionIncidentes.Application.Interfaces;
using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Queries;

public class GetTechnicianWorkloadQueryHandler : IRequestHandler<GetTechnicianWorkloadQuery, List<TechnicianWorkloadDto>>
{
    private readonly IUserRepository _userRepo;
    private readonly ITicketRepository _ticketRepo;
    private readonly IDepartmentRepository _deptRepo;

    public GetTechnicianWorkloadQueryHandler(
        IUserRepository userRepo,
        ITicketRepository ticketRepo,
        IDepartmentRepository deptRepo)
    {
        _userRepo = userRepo;
        _ticketRepo = ticketRepo;
        _deptRepo = deptRepo;
    }

    public async Task<List<TechnicianWorkloadDto>> Handle(GetTechnicianWorkloadQuery request, CancellationToken cancellationToken)
    {
        var allUsers = await _userRepo.GetAllAsync();
        var technicians = allUsers.Where(u => u.Role == "Tecnico").ToList();
        
        var allTickets = await _ticketRepo.ListAllAsync();
        
        var result = new List<TechnicianWorkloadDto>();
        
        foreach (var tech in technicians)
        {
            var techTickets = allTickets.Where(t => t.AssignedToUserId == tech.Id).ToList();
            
            var activeTickets = techTickets.Count(t => t.Status == "En proceso");
            var pendingTickets = techTickets.Count(t => t.Status != "Resuelto" && t.Status != "Cerrado");
            
            var today = DateTime.UtcNow.Date;
            var resolvedToday = techTickets.Count(t => 
                t.ResolvedAt.HasValue && 
                t.ResolvedAt.Value.Date == today);
            
            var totalResolved = techTickets.Count(t => t.Status == "Resuelto" || t.Status == "Cerrado");
            
            var dept = await _deptRepo.GetByIdAsync(tech.DepartmentId);
            
            result.Add(new TechnicianWorkloadDto(
                tech.Id,
                tech.FullName,
                dept?.Name ?? "Sin departamento",
                activeTickets,
                pendingTickets,
                resolvedToday,
                totalResolved
            ));
        }
        
        return result.OrderBy(t => t.ActiveTickets).ToList(); // Ordenar por carga menor primero
    }
}
