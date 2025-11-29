using GestionIncidentes.Application.Interfaces;
using System.Text;

namespace GestionIncidentes.Infrastructure.Services;

/// <summary>
/// Servicio para formatear reportes en HTML/Markdown (luego se puede exportar a PDF)
/// </summary>
public class ReportFormatter
{
    private readonly ITicketReportRepository _reportRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public ReportFormatter(
        ITicketReportRepository reportRepository,
        ITicketRepository ticketRepository,
        IUserRepository userRepository)
    {
        _reportRepository = reportRepository;
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<string> FormatReportAsHtmlAsync(Guid reportId)
    {
        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null) return string.Empty;

        var ticket = await _ticketRepository.GetByIdAsync(report.TicketId);
        var technician = await _userRepository.GetByIdAsync(report.TechnicianId);

        var sb = new StringBuilder();
        sb.AppendLine("<html><head><title>Reporte de Ticket</title></head><body>");
        sb.AppendLine($"<h1>Reporte de Ticket #{report.TicketId}</h1>");
        sb.AppendLine($"<p><strong>Título:</strong> {ticket?.Title}</p>");
        sb.AppendLine($"<p><strong>Técnico:</strong> {technician?.FullName}</p>");
        sb.AppendLine($"<p><strong>Fecha:</strong> {report.CreatedAt:yyyy-MM-dd HH:mm}</p>");
        sb.AppendLine($"<h2>Diagnóstico del Problema</h2>");
        sb.AppendLine($"<p>{report.ProblemDiagnosis}</p>");
        sb.AppendLine($"<h2>Acciones Realizadas</h2>");
        sb.AppendLine($"<p>{report.ActionsTaken}</p>");
        sb.AppendLine($"<h2>Detalles de Resolución</h2>");
        sb.AppendLine($"<p>{report.ResolutionDetails}</p>");
        sb.AppendLine($"<p><strong>Tiempo Empleado:</strong> {report.TimeSpent}</p>");
        sb.AppendLine("</body></html>");

        return sb.ToString();
    }

    public async Task<string> FormatReportAsMarkdownAsync(Guid reportId)
    {
        var report = await _reportRepository.GetByIdAsync(reportId);
        if (report == null) return string.Empty;

        var ticket = await _ticketRepository.GetByIdAsync(report.TicketId);
        var technician = await _userRepository.GetByIdAsync(report.TechnicianId);

        var sb = new StringBuilder();
        sb.AppendLine($"# Reporte de Ticket #{report.TicketId}");
        sb.AppendLine();
        sb.AppendLine($"**Título:** {ticket?.Title}");
        sb.AppendLine($"**Técnico:** {technician?.FullName}");
        sb.AppendLine($"**Fecha:** {report.CreatedAt:yyyy-MM-dd HH:mm}");
        sb.AppendLine();
        sb.AppendLine("## Diagnóstico del Problema");
        sb.AppendLine(report.ProblemDiagnosis);
        sb.AppendLine();
        sb.AppendLine("## Acciones Realizadas");
        sb.AppendLine(report.ActionsTaken);
        sb.AppendLine();
        sb.AppendLine("## Detalles de Resolución");
        sb.AppendLine(report.ResolutionDetails);
        sb.AppendLine();
        sb.AppendLine($"**Tiempo Empleado:** {report.TimeSpent}");

        return sb.ToString();
    }
}
