using System;

namespace GestionIncidentes.Domain.Events;

public sealed record IncidentReported(Guid IncidentId, Guid ReportedByUserId);
public sealed record TicketCreated(Guid TicketId, Guid IncidentId, Guid? AssignedToUserId);
public sealed record TicketAssigned(Guid TicketId, Guid IncidentId, Guid AssignedToUserId);
public sealed record TicketResolved(Guid TicketId, Guid IncidentId, Guid ResolvedByUserId);
public sealed record KnowledgeEntryAdded(Guid KnowledgeEntryId, Guid CreatedByUserId);