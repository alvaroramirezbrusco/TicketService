using Application.Interfaces.ITicket;
using Application.Models.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tickets.Queries
{
    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, List<TicketResponse>>
    {
        private readonly ITicketQuery _ticketQuery;

        public GetAllTicketsQueryHandler(ITicketQuery ticketQuery)
        {
            _ticketQuery = ticketQuery;
        }

        public async Task<List<TicketResponse>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketQuery.GetTicketAllAsync(request.eventId, request.userId);
            if (tickets == null || !tickets.Any())
            {
                if (request.eventId.HasValue && request.userId.HasValue)
                {
                    throw new KeyNotFoundException($"No se encontraron tickets para el evento con ID {request.eventId.Value} y el usuario con ID {request.userId.Value}.");
                }
                if (request.eventId.HasValue)
                {
                    throw new KeyNotFoundException($"No se encontraron tickets para el evento con ID {request.eventId.Value}.");
                }
                if (request.userId.HasValue)
                {
                    throw new KeyNotFoundException($"No se encontraron tickets para el usuario con ID {request.userId.Value}.");
                }                
                if (!request.eventId.HasValue && !request.userId.HasValue)
                {
                    throw new KeyNotFoundException("No se encontraron tickets cargados en el sistema.");
                }
            }
            return tickets.Select(ticket => new TicketResponse
            {
                TicketId = ticket.TicketId,
                UserId = ticket.UserId,
                EventId = ticket.EventId,
                Created = ticket.Created,
                Updated = ticket.Updated,
                Status = new TicketStatusResponse
                {
                    StatusID = ticket.StatusRef.StatusID,
                    Name = ticket.StatusRef.Name,
                },
                TicketSeats = ticket.TicketSeats.Select(ts => new TicketSeatResponse
                {
                    TicketSeatId = ts.TicketSeatId,
                    TicketId = ts.TicketId,
                    EventSeatId = ts.EventSeatId,
                }).ToList(),
                TicketSectors = ticket.TicketSectors.Select(ts => new TicketSectorResponse
                {
                    TicketSectorId = ts.TicketSectorId,
                    TicketId = ts.TicketSectorId,
                    Quantity = ts.Quantity
                }).ToList()
            }).ToList();
        }
    }
}
