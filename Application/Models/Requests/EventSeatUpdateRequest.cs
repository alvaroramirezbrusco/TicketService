using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class EventSeatUpdateRequest
    {
        public Guid EventSeatId { get; set; }
        public bool Reserved { get; set; }
        public Guid? ReserverByUserId { get; set; }
    }
}
