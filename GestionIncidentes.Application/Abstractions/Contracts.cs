using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestionIncidentes.Application.Abstractions;

// Infra-agnostic "ports"
public interface IIncidentRepository
{
    Task<IncidentReadModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(IncidentWriteModel incident, CancellationToken ct = default);
    Task<List<IncidentReadModel>> ListAsync(IncidentFilter filter, CancellationToken ct = default);
}

public interface ITicketRepository
{
    Task<TicketReadModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(TicketWriteModel ticket, CancellationToken ct = default);
    Task UpdateAsync(TicketWriteModel ticket, CancellationToken ct = default);
    Task<List<TicketReadModel>> ListByAssigneeAsync(Guid userId, TicketStatus? status, CancellationToken ct = default);
}

public interface IUserRepository
{
    Task<UserReadModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<UserReadModel?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(UserWriteModel user, CancellationToken ct = default);
    Task<List<UserReadModel>> ListAsync(UserFilter filter, CancellationToken ct = default);
}

public interface IRoleRepository
{
    Task<List<RoleReadModel>> ListAsync(CancellationToken ct = default);
}

public interface IDepartmentRepository
{
    Task<List<DepartmentReadModel>> ListAsync(CancellationToken ct = default);
}

public interface IKnowledgeRepository
{
    Task AddAsync(KnowledgeEntryWriteModel entry, CancellationToken ct = default);
    Task<List<KnowledgeEntryReadModel>> SearchAsync(string query, string[]? tags, CancellationToken ct = default);
}

public interface IAuditLogRepository
{
    Task AddAsync(AuditLogWriteModel entry, CancellationToken ct = default);
    Task<List<AuditLogReadModel>> ListAsync(AuditFilter filter, CancellationToken ct = default);
}

public interface INotificationService
{
    Task NotifyAsync(NotificationMessage message, CancellationToken ct = default);
}

public interface IWorkloadService
{
    Task<WorkloadScoreReadModel> CalculateForUserAsync(Guid userId, CancellationToken ct = default);
    Task<bool> CanAssignAsync(Guid userId, TicketPriority priority, CancellationToken ct = default);
}

public interface IReportFormatter
{
    Task<string> BuildTicketReportAsync(TicketReadModel ticket, IncidentReadModel incident, CancellationToken ct = default);
}

public interface ICurrentUser
{
    Guid? UserId { get; }
    string? Email { get; }
    bool IsInRole(string role);
    int RoleLevel { get; } // Para jerarquía (técnico < coordinador < DITIC)
}

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

// DTOs y modelos “read/write” (UI-friendly, sin EF)
public enum TicketStatus { New, InProgress, Resolved, Closed }
public enum TicketPriority { Low, Medium, High }

public sealed record IncidentWriteModel(Guid Id, string Type, string Description, Guid ReportedByUserId, DateTime ReportedAtUtc);
public sealed record IncidentReadModel(Guid Id, string Type, string Description, Guid ReportedByUserId, DateTime ReportedAtUtc, string State);

public sealed record TicketWriteModel(Guid Id, Guid IncidentId, Guid? AssignedToUserId, TicketStatus Status, TicketPriority Priority, DateTime CreatedAtUtc);
public sealed record TicketReadModel(Guid Id, Guid IncidentId, Guid? AssignedToUserId, TicketStatus Status, TicketPriority Priority, DateTime CreatedAtUtc);

public sealed record UserWriteModel(Guid Id, string Name, string Email, Guid DepartmentId, int RoleLevel);
public sealed record UserReadModel(Guid Id, string Name, string Email, Guid DepartmentId, int RoleLevel, string[] Roles);
public sealed record RoleReadModel(Guid Id, string Name, int Level);
public sealed record DepartmentReadModel(Guid Id, string Name, Guid? ManagerUserId);
public sealed record WorkloadScoreReadModel(Guid UserId, double Score, int ActiveTickets);

public sealed record KnowledgeEntryWriteModel(Guid Id, string Title, string Problem, string Solution, string[] Tags, Guid CreatedByUserId, DateTime CreatedAtUtc);
public sealed record KnowledgeEntryReadModel(Guid Id, string Title, string Problem, string Solution, string[] Tags, Guid CreatedByUserId, DateTime CreatedAtUtc);

public sealed record NotificationMessage(Guid UserId, string Title, string Body);

public sealed record AuditLogWriteModel(Guid Id, string Entity, Guid EntityId, string Action, Guid PerformedByUserId, DateTime TimestampUtc, string? Before, string? After);
public sealed record AuditLogReadModel(Guid Id, string Entity, Guid EntityId, string Action, Guid PerformedByUserId, DateTime TimestampUtc);

public sealed record IncidentFilter(string? Type, string? State, Guid? ReportedByUserId);
public sealed record UserFilter(string? Email, Guid? DepartmentId);
public sealed record AuditFilter(string? Entity, Guid? EntityId);