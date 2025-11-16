using Application.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EventSeat.Queries
{
    public record GetAllEventSeatQuery() : IRequest<List<EventSeatResponse>?>;
}
