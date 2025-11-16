using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EventSector
    {
        public Guid EventSectorId { get; set; }
        public Guid EventId { get; set; }
        public Guid SectorId { get; set; }
        public string Name { get; set; }
        public bool IsControlled { get; set; }
        public int Capacity { get; set; }
        public int SoldCount { get; set; }
        public int ReservedCount { get; set; }
        public decimal Price { get; set; }

        // Relationships
        public ICollection<EventSeat> EventSeats { get; set; }
        public ICollection<TicketSector> TicketSectors { get; set; }
    }
}
