using Application.Interfaces.IEventSector;
using Application.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EventSector.Commands
{
    public class UpdateEventSectorCapacityCommandHandler : IRequestHandler<UpdateEventSectorCapacityCommand, EventSectorReservedResponse>
    {
        private readonly IEventSectorCommand _eventSectorCommand;
        private readonly IEventSectorQuery _eventSectorQuery;

        public UpdateEventSectorCapacityCommandHandler(IEventSectorCommand eventSectorCommand, IEventSectorQuery eventSectorQuery)
        {
            _eventSectorCommand = eventSectorCommand;
            _eventSectorQuery = eventSectorQuery;
        }

        public async Task<EventSectorReservedResponse> Handle(UpdateEventSectorCapacityCommand request, CancellationToken cancellationToken)
        {
            var sector = await _eventSectorQuery.GetEventSectorByIdAsync(request.EventSectorId);
            if (sector == null)
            {
                throw new KeyNotFoundException("Sector no encontrado.");
            }
            if (sector.IsControlled)
            {
                throw new ArgumentException("Este endpoint solo sirve para sectores no controlados.");
            }
            if (request.Reserve)
            {
                // reserva
                if (sector.SoldCount + sector.ReservedCount + request.Quantity > sector.Capacity)
                {
                    throw new ArgumentException("No hay suficiente capacidad en el sector para reservar la cantidad solicitada.");
                }
                sector.ReservedCount += request.Quantity;
            }
            else
            {
                // libera
                if (sector.ReservedCount < request.Quantity)
                {
                    throw new ArgumentException("No hay suficientes reservas en el sector para liberar la cantidad solicitada.");
                }
                sector.ReservedCount -= request.Quantity;
            }

            await _eventSectorCommand.UpdateSector(sector);

            return new EventSectorReservedResponse
            {
                EventSectorId = sector.EventSectorId,
                EventId = sector.EventId,
                Name = sector.Name,
                IsControlled = sector.IsControlled,
                Capacity = sector.Capacity,
                SoldCount = sector.SoldCount,
                ReservedCount = sector.ReservedCount,
                Price = sector.Price                
            };
        }
    }
}
