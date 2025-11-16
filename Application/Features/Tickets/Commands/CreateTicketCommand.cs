using MediatR;
using Application.Models.Requests;
using Application.Models.Responses;

namespace Application.Features.Tickets.Commands
{
    public record CreateTicketCommand(TicketRequest Request) : IRequest<TicketResponse>;
}
