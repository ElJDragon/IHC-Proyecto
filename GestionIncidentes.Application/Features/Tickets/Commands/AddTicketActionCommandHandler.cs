using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Tickets.Commands;

public class AddTicketActionCommandHandler : IRequestHandler<AddTicketActionCommand, Unit>
{
    private readonly ITicketRepository _ticketRepo;
    private readonly ICurrentUser _currentUser;
    private readonly IAuditLogRepository _auditLogRepo;

    public AddTicketActionCommandHandler(
        ITicketRepository ticketRepo,
        ICurrentUser currentUser,
        IAuditLogRepository auditLogRepo)
    {
        _ticketRepo = ticketRepo;
        _currentUser = currentUser;
        _auditLogRepo = auditLogRepo;
    }

    public async Task<Unit> Handle(AddTicketActionCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepo.GetByIdAsync(request.TicketId);
        if (ticket == null)
            throw new KeyNotFoundException("Ticket no encontrado");

        var userId = _currentUser.UserId ?? throw new UnauthorizedAccessException("Usuario no autenticado");

        // Registrar la acción en el log de auditoría
        var auditLog = AuditLog.Create(
            userId,
            $"TicketAction:{request.Action}",
            "Action",
            request.TicketId,
            request.Details
        );

        await _auditLogRepo.AddAsync(auditLog);

        return Unit.Value;
    }
}
