using MediatR;

namespace GestionIncidentes.Application.Features.Reports;

public record GenerateTicketReportQuery(
    Guid TicketId,
    Guid TechnicianId,
    string ResolutionDetails,
    string ProblemDiagnosis,
    string ActionsTaken,
    TimeSpan TimeSpent,
    bool SuggestKnowledgeEntry = false
) : IRequest<Guid>;
