using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EventSector.Commands
{
    public record CreateEventSectorCommand(EventSectorCreateRequest Request) : IRequest<List<EventSectorResponse>>;
}
