using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Features.Tickets.Commands
{
    public record UpdateTicketCommand(Guid Id, TicketUpdateRequest Request) : IRequest<TicketResponse>;
}
