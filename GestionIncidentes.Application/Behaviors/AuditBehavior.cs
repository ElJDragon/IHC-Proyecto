using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;
using System.Text.Json;

namespace GestionIncidentes.Application.Behaviors;

/// <summary>
/// Pipeline behavior que registra automáticamente todas las operaciones en el AuditLog
/// </summary>
public class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ICurrentUser _currentUser;

    public AuditBehavior(IAuditLogRepository auditLogRepository, ICurrentUser currentUser)
    {
        _auditLogRepository = auditLogRepository;
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? Guid.Empty;
        var requestName = typeof(TRequest).Name;

        // Ejecutar el request
        var response = await next();

        // Registrar en auditoría (solo comandos, no queries por performance)
        if (requestName.EndsWith("Command"))
        {
            var details = JsonSerializer.Serialize(request);
            var auditLog = AuditLog.Create(
                userId,
                requestName,
                "Command",
                null,
                details
            );

            await _auditLogRepository.AddAsync(auditLog);
        }

        return response;
    }
}
