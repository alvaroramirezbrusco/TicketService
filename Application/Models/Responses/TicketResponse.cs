using Application.Models.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses
{
    public class TicketResponse
    {
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public TicketStatusResponse Status { get; set; }
        public List<TicketSeatResponse>? TicketSeats { get; set; }
        public List<TicketSectorResponse>? TicketSectors { get; set; }
        
    }
}
