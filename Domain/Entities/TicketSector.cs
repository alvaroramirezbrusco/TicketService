using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TicketSector
    {
        public Guid TicketSectorId { get; set; }
        public Guid TicketId { get; set; }
        public Guid EventSectorId { get; set; }
        public int Quantity { get; set; }

        public Ticket TicketRef { get; set; }
        public EventSector EventSectorRef { get; set; }
    }
}
