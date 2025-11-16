using Application.Interfaces.IEvenSeat;
using Application.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EventSeat.Commands
{
    public class UpdateEventSeatStatusCommandHandler : IRequestHandler<UpdateEventSeatStatusCommand, EventSeatReservedResponse>
    {
        private readonly IEventSeatCommand _eventSeatCommand;
        private readonly IEventSeatQuery _eventSeatQuery;

        public UpdateEventSeatStatusCommandHandler(IEventSeatCommand eventSeatCommand, IEventSeatQuery eventSeatQuery)
        {
            _eventSeatCommand = eventSeatCommand;
            _eventSeatQuery = eventSeatQuery;
        }

        public async Task<EventSeatReservedResponse> Handle(UpdateEventSeatStatusCommand request, CancellationToken cancellationToken)
        {
            var dto = request.request;
            
            var eventSeatExist = await _eventSeatQuery.GetEventSeatById(dto.EventSeatId);
            if (eventSeatExist == null)
            {
                throw new ArgumentException($"El asiento con Id {request.request.EventSeatId} no se encontro");
            }
            
            if(dto.Reserved)
            {
                if (dto.ReserverByUserId == null)
                {
                    throw new ArgumentException("Para reservar un asiento debe ingresar el Id del usuario que realiza la reserva");
                }
                if (dto.ReserverByUserId == eventSeatExist.ReservedByUserId)
                {
                    throw new ArgumentException("El asiento ya se encuentra reservado por el mismo usuario");
                }
                if (eventSeatExist.StatusId == 3)
                {
                    throw new ArgumentException("No se puede reservar un asiento que ya fue vendido");
                }
                if (eventSeatExist.StatusId == 2 && eventSeatExist.ReservedByUserId != dto.ReserverByUserId)
                {
                    throw new ArgumentException("El asiento ya se encuentra reservado por otro usuario");
                }                
                eventSeatExist.ReservedByUserId = dto.ReserverByUserId;
                eventSeatExist.StatusId = 2; // estado pasa a reservado
                await _eventSeatCommand.UpdateEventSeat(eventSeatExist);
                eventSeatExist = await _eventSeatQuery.GetEventSeatById(dto.EventSeatId);
            }
            else
            {
                if (dto.ReserverByUserId == null || eventSeatExist.ReservedByUserId != dto.ReserverByUserId)
                {
                    throw new ArgumentException("Para eliminar la reserva de un asiento debe ingresar el Id del usuario que realiza la reserva");
                }
                eventSeatExist.ReservedByUserId = null;
                eventSeatExist.StatusId = 1; // estado pasa a habilitado
                await _eventSeatCommand.UpdateEventSeat(eventSeatExist);
                eventSeatExist = await _eventSeatQuery.GetEventSeatById(dto.EventSeatId);
            }

            return new EventSeatReservedResponse
            {
                Message = "El estado del asiento se actualizo correctamente",
                EventSeatId = eventSeatExist.EventSeatId,
                EventId = eventSeatExist.EventId,
                EventSectorId = eventSeatExist.EventSectorId,
                SeatId = eventSeatExist.SeatId,
                Price = eventSeatExist.Price,
                Status = new TicketStatusResponse
                {
                    StatusID = eventSeatExist.StatusId,
                    Name = eventSeatExist.StatusRef.Name
                },
                ReserverByUserId = eventSeatExist.ReservedByUserId
            };
        }
    }
}
