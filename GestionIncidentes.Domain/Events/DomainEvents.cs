using System;
using MediatR;

namespace GestionIncidentes.Domain.Events;

// Eventos del Integrante A - Incidents y Tickets
public sealed record IncidentReported(Guid IncidentId, string Title, Guid ReportedByUserId) : INotification;
public sealed record TicketCreated(Guid TicketId, string Title, Guid CreatedByUserId) : INotification;
public sealed record TicketAssigned(Guid TicketId, Guid AssignedToUserId) : INotification;
public sealed record TicketResolved(Guid TicketId) : INotification;

// Eventos del Integrante C
public sealed record KnowledgeEntryAdded(Guid KnowledgeEntryId, Guid CreatedByUserId) : INotification;