

namespace Application.Models.Requests
{
    public class EventSectorCreateRequest
    {        
        public Guid EventId { get; set; }
        public List<EventSectorItemRequest> Sectors { get; set; }
    }
}
