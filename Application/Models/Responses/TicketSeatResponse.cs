using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses
{
    public class TicketSeatResponse
    {
        public Guid TicketSeatId { get; set; }
        public Guid TicketId { get; set; }
        public Guid EventSeatId { get; set; }
    }
}
