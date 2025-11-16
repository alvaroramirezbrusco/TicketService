using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class TicketSectorRequest
    {
        public Guid EventSectorId { get; set; }
        public int Quantity { get; set; }
    }
}
