using Application.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EventSector.Queries
{
    public record GetEventSectorByIdQuery(Guid EventSectorId) : IRequest<EventSectorResponse>;
}
