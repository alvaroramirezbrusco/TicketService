using Application.Interfaces.IEventSector;
using Application.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EventSector.Queries
{
    public class GetEventSectorByIdQueryHandler : IRequestHandler<GetEventSectorByIdQuery, EventSectorResponse>
    {
        private readonly IEventSectorQuery _eventSectorQuery;

        public GetEventSectorByIdQueryHandler(IEventSectorQuery eventSectorQuery)
        {
            _eventSectorQuery = eventSectorQuery;
        }

        public async Task<EventSectorResponse> Handle(GetEventSectorByIdQuery request, CancellationToken cancellationToken)
        {
            var sector = await _eventSectorQuery.GetEventSectorByIdAsync(request.EventSectorId);
            if (sector == null)
            {
                throw new KeyNotFoundException("No existe un sector con el ID proporcionado.");
            }

            return new EventSectorResponse
            {
                EventSectorId = sector.EventSectorId,
                EventId = sector.EventId,
                Name = sector.Name,
                IsControlled = sector.IsControlled,
                Capacity = sector.Capacity,
                Price = sector.Price
            };
        }
    }
}
