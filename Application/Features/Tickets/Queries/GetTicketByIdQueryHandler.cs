using Application.Interfaces.ITicket;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tickets.Queries
{
    public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketResponse>
    {
        private readonly ITicketQuery _ticketQuery;

        public GetTicketByIdQueryHandler(ITicketQuery ticketQuery)
        {
            _ticketQuery = ticketQuery;
        }

        public async Task<TicketResponse> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await _ticketQuery.GetTicketById(request.ticketId);
            if (dto == null)
            {
                throw new KeyNotFoundException($"No se encontro el ticket con ID {request.ticketId}");
            }
            return new TicketResponse
            {
                TicketId = dto.TicketId,
                UserId = dto.UserId,
                EventId = dto.EventId,
                Created = dto.Created,
                Updated = dto.Updated,
                Status = new TicketStatusResponse
                {
                    StatusID = dto.StatusRef.StatusID,
                    Name = dto.StatusRef.Name,
                },
                TicketSeats = dto.TicketSeats.Select(ts => new TicketSeatResponse
                {
                    TicketSeatId = ts.TicketSeatId,
                    TicketId = ts.TicketId,
                    EventSeatId = ts.EventSeatId,
                }).ToList(),
                TicketSectors = dto.TicketSectors.Select(ts => new TicketSectorResponse
                {
                    TicketSectorId = ts.TicketSectorId,
                    TicketId = ts.TicketSectorId,
                    Quantity = ts.Quantity
                }).ToList()
            };
        }
    }
}
