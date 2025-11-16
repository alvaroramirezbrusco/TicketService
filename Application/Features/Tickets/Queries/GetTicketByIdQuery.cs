using Application.Models.Responses;
using MediatR;

namespace Application.Features.Tickets.Queries
{
    public record GetTicketByIdQuery(Guid ticketId) : IRequest<TicketResponse>;
}
