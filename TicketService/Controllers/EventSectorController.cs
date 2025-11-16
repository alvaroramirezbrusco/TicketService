using Application.Features.EventSector.Commands;
using Application.Features.EventSector.Queries;
using Application.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TicketService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventSectorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventSectorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventSectorCreateRequest Request)
        {
            var result = await _mediator.Send(new CreateEventSectorCommand(Request));
            return Ok(result);
        }

        [HttpGet("eventSectorId")]
        public async Task<IActionResult> GetById(Guid eventSectorId)
        {
            var sector = await _mediator.Send(new GetEventSectorByIdQuery(eventSectorId));
            return Ok(sector);
        }

        [HttpGet("event")]
        public async Task<IActionResult> GetbyEventId(Guid eventId)
        {
            var sectors = await _mediator.Send(new GetEventSectorByEventIdQuery(eventId));
            return Ok(sectors);
        }

        [HttpPatch("{eventSectorId:guid}")]
        public async Task<IActionResult> UpdateCapacity(Guid eventSectorId, [FromBody] EventSectorCapacityUpdateRequest request)
        {
            var result = await _mediator.Send(new UpdateEventSectorCapacityCommand(eventSectorId, request.Reserve, request.Quantity));
            return Ok(result);
        }
    }
}
