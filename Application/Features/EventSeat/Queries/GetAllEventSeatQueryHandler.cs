using Application.Interfaces.IEvenSeat;
using Application.Models.Responses;
using MediatR;

namespace Application.Features.EventSeat.Queries
{
    public class GetAllEventSeatQueryHandler : IRequestHandler<GetAllEventSeatQuery, List<EventSeatResponse>?>
    {
        private readonly IEventSeatQuery _eventSeatQuery;

        public GetAllEventSeatQueryHandler(IEventSeatQuery eventSeatQuery)
        {
            _eventSeatQuery = eventSeatQuery;
        }

        public async Task<List<EventSeatResponse>?> Handle(GetAllEventSeatQuery request, CancellationToken cancellationToken)
        {
            var eventSeats = await _eventSeatQuery.GetEventSeatsAllAsync();
            if(eventSeats == null || eventSeats.Count == 0)
            {
                throw new KeyNotFoundException("No se encontraron asientos de eventos registrados en el sistema");
            }
            return eventSeats.Select(eventSeat => new EventSeatResponse
            {
                EventSeatId = eventSeat.EventSeatId,
                EventId = eventSeat.EventId,
                EventSectorId = eventSeat.EventSectorId,
                SeatId = eventSeat.SeatId,
                Price = eventSeat.Price,
                Status = new TicketStatusResponse
                {
                    StatusID = eventSeat.StatusId,
                    Name = eventSeat.StatusRef.Name
                },
                ReserverByUserId = eventSeat.ReservedByUserId
            }).ToList();
        }
    }
}
