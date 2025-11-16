using Application.Features.Tickets.Commands;
using Application.Features.Tickets.Queries;
using Application.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TicketService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(TicketRequest request)
        {
            var result = await _mediator.Send(new CreateTicketCommand(request));
            return CreatedAtAction(nameof(GetTicketById), new { id = result.TicketId }, result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id,[FromBody] TicketUpdateRequest request)
        {
            var result = await _mediator.Send(new UpdateTicketCommand(id, request));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTicketById(Guid id)
        {
            var result = await _mediator.Send(new GetTicketByIdQuery(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlltickets([FromQuery] Guid? eventId, [FromQuery] Guid? userId)
        {
            var items = await _mediator.Send(new GetAllTicketsQuery(eventId, userId));
            return Ok(items);
        }
    }
}
