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
    public class GetEventSectorByEventIdQueryHandler : IRequestHandler<GetEventSectorByEventIdQuery, List<EventSectorResponse>>
    {
        private readonly IEventSectorQuery _eventSectorQuery;

        public GetEventSectorByEventIdQueryHandler(IEventSectorQuery eventSectorQuery)
        {
            _eventSectorQuery = eventSectorQuery;
        }

        public async Task<List<EventSectorResponse>> Handle(GetEventSectorByEventIdQuery request, CancellationToken cancellationToken)
        {
            var sectors = await _eventSectorQuery.GetEventSectorByEventIdAsync(request.EventId);
            return sectors.Select(s => new EventSectorResponse
            {
                EventSectorId = s.EventSectorId,
                EventId = s.EventId,
                Name = s.Name,
                IsControlled = s.IsControlled,
                Capacity = s.Capacity,
                Price = s.Price
            }).ToList();
        }
    }
}
