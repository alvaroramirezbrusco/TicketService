using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses
{
    public class EventSeatCreatedResponse
    {
        public Guid EventSeatId { get; set; }
        public long? SeatId { get; set; }
    }
}
