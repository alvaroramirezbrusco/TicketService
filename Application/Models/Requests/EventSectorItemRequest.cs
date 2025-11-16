using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class EventSectorItemRequest
    {
        public Guid SectorId { get; set; }
        public string Name { get; set; }
        public bool IsControlled { get; set; }
        public int? Capacity { get; set; }
        public decimal Price { get; set; }
        public List<long>? Seats { get; set; }
    }
}
