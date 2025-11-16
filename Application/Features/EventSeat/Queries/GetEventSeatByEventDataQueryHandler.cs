using Application.Interfaces.IEvenSeat;
using Application.Models.Responses;
using MediatR;
using System.Reflection.Metadata.Ecma335;

namespace Application.Features.EventSeat.Queries
{
    public class GetEventSeatByEventDataQueryHandler : IRequestHandler<GetEventSeatByEventDataQuery, EventSeatResponse?>
    {
        private readonly IEventSeatQuery _eventSeatRepository;

        public GetEventSeatByEventDataQueryHandler(IEventSeatQuery eventSeatRepository)
        {
            _eventSeatRepository = eventSeatRepository;
        }
              
        public async Task<EventSeatResponse?> Handle(GetEventSeatByEventDataQuery request, CancellationToken cancellationToken)
        {
            var eventSeat = await _eventSeatRepository.GetEventSeatByEventSectorIdAsync(request.eventId, request.eventSectorId, request.seatId);
            if (eventSeat == null)
            {                                              
                if (request.eventId == Guid.Empty)
                {
                    throw new ArgumentException("Debe ingresar un Id de evento");
                }
                if (request.eventSectorId == Guid.Empty)
                {
                    throw new ArgumentException("Debe ingresar un Id de sector de evento");
                }
                if (request.seatId == 0)
                {
                    throw new ArgumentException("Debe ingresar un Id de asiento");
                }
                if (request.seatId <= 0)
                {
                    throw new ArgumentException("El Id de asiento no es valido");
                }
                if (request.eventId != Guid.Empty && request.eventSectorId != Guid.Empty && request.seatId != 0)
                {
                    throw new KeyNotFoundException("No se encontró ningún asiento para los datos proporcionados");
                }
            }
    
            return new EventSeatResponse
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
            };
        }
    }
}
