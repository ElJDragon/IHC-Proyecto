using System;
using MediatR;

namespace GestionIncidentes.Domain.Events;

public sealed record IncidentReported(Guid IncidentId, Guid ReportedByUserId) : INotification;
public sealed record TicketCreated(Guid TicketId, Guid IncidentId, Guid? AssignedToUserId) : INotification;
public sealed record TicketAssigned(Guid TicketId, Guid IncidentId, Guid AssignedToUserId) : INotification;
public sealed record TicketResolved(Guid TicketId, Guid IncidentId, Guid ResolvedByUserId) : INotification;
public sealed record KnowledgeEntryAdded(Guid KnowledgeEntryId, Guid CreatedByUserId) : INotification;