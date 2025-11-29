using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using MediatR;

namespace GestionIncidentes.Application.Features.Reports;

public class GenerateTicketReportQueryHandler : IRequestHandler<GenerateTicketReportQuery, Guid>
{
    private readonly ITicketReportRepository _reportRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IAuditLogRepository _auditLogRepository;

    public GenerateTicketReportQueryHandler(
        ITicketReportRepository reportRepository,
        ITicketRepository ticketRepository,
        IAuditLogRepository auditLogRepository)
    {
        _reportRepository = reportRepository;
        _ticketRepository = ticketRepository;
        _auditLogRepository = auditLogRepository;
    }

    public async Task<Guid> Handle(GenerateTicketReportQuery request, CancellationToken cancellationToken)
    {
        var report = TicketReport.Create(
            request.TicketId,
            request.TechnicianId,
            request.ResolutionDetails,
            request.ProblemDiagnosis,
            request.ActionsTaken,
            request.TimeSpent,
            request.SuggestKnowledgeEntry
        );

        await _reportRepository.AddAsync(report);

        // Auditoría
        var auditLog = AuditLog.Create(
            request.TechnicianId,
            "GeneratedTicketReport",
            "TicketReport",
            report.Id,
            $"TicketId: {request.TicketId}, TimeSpent: {request.TimeSpent}"
        );
        await _auditLogRepository.AddAsync(auditLog);

        return report.Id;
    }
}
