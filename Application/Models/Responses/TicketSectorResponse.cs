using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses
{
    public class TicketSectorResponse
    {
        public Guid TicketSectorId { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
