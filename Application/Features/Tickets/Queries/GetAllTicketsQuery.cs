using Application.Models.Responses;
using MediatR;

namespace Application.Features.Tickets.Queries
{
    public record GetAllTicketsQuery(Guid? eventId, Guid? userId) : IRequest<List<TicketResponse>>;
}
