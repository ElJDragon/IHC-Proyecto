using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using GestionIncidentes.Domain.Events;
using MediatR;

namespace GestionIncidentes.Application.EventHandlers;

/// <summary>
/// Maneja el evento OnIncidentReported para notificar al DITIC
/// </summary>
public class OnIncidentReportedHandler : INotificationHandler<IncidentReported>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;

    public OnIncidentReportedHandler(
        INotificationRepository notificationRepository,
        IUserRepository userRepository)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(IncidentReported notification, CancellationToken cancellationToken)
    {
        // Obtener usuarios con rol DITIC (o encargados)
        var allUsers = await _userRepository.GetAllAsync();
        var diticUsers = allUsers.Where(u => u.Role.Equals("DITIC", StringComparison.OrdinalIgnoreCase) 
                                          || u.Role.Equals("Encargado", StringComparison.OrdinalIgnoreCase));

        foreach (var diticUser in diticUsers)
        {
            var notif = Notification.Create(
                diticUser.Id,
                "Nuevo Incidente Reportado",
                $"Se ha reportado un nuevo incidente que requiere asignación.",
                "IncidentReported",
                notification.IncidentId
            );

            await _notificationRepository.AddAsync(notif);
        }
    }
}
