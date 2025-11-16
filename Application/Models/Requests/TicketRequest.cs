using Application.Models.Requests;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class TicketRequest
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public List<Guid>? EventSeatIds { get; set; }
        public List<TicketSectorRequest> Sectors { get; set; }
    }
}