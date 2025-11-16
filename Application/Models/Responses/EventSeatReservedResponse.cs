using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses
{
    public class EventSeatReservedResponse
    {
        public string Message { get; set; }
        public Guid EventSeatId { get; set; }
        public Guid EventId { get; set; }
        public Guid EventSectorId { get; set; }
        public long? SeatId { get; set; }
        public Decimal Price { get; set; }
        public TicketStatusResponse Status { get; set; }
        public Guid? ReserverByUserId { get; set; }
    }
}
