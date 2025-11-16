using Application.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EventSector.Commands
{
    public record UpdateEventSectorCapacityCommand(Guid EventSectorId, bool Reserve, int Quantity) : IRequest<EventSectorReservedResponse>;
}
