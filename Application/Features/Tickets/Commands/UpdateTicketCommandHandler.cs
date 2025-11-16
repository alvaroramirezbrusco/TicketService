using Application.Interfaces.ITicket;
using Application.Interfaces.ITicketStatus;
using Application.Models.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tickets.Commands
{
    public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, TicketResponse>
    {
        private readonly ITicketCommand _ticketCommand;
        private readonly ITicketQuery _ticketQuery;
        private readonly ITicketStatusQuery _statusQuery;

        public UpdateTicketCommandHandler(ITicketCommand command, ITicketQuery query, ITicketStatusQuery statusQuery)
        {
            _ticketCommand = command;
            _ticketQuery = query;
            _statusQuery = statusQuery;
        }

        public async Task<TicketResponse> Handle(UpdateTicketCommand Request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketQuery.GetTicketById(Request.Id);
            if (ticket == null)
            {
                throw new ArgumentNullException($"El Ticket no existe para el Id ingresado: {Request.Id}");
            }

            var statusRef = await _statusQuery.GetTicketStatusById(Request.Request.StatusId);
            if (statusRef == null)
            {
                throw new ArgumentNullException($"El estado no existe para el Id ingresado: {Request.Id}");
            }

            ticket.StatusRef = statusRef;
            ticket.StatusId = statusRef.StatusID;
            ticket.Updated = DateTime.UtcNow;
            await _ticketCommand.UpdateTicket(ticket);

            return new TicketResponse
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
            };
        }
    }
}
