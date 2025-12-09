using MediatR;
using GestionIncidentes.Application.Features.Tickets.Dtos;

namespace GestionIncidentes.Application.Features.Tickets.Queries;

public record GetAllTicketsQuery : IRequest<List<TicketDto>>;
