using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IEvenSeat
{
    public interface IEventSeatQuery
    {
        Task<EventSeat?> GetEventSeatById(Guid id);
        Task<List<EventSeat>> GetEventSeatsAllAsync();
        Task<EventSeat> GetEventSeatByEventSectorIdAsync(Guid? eventId, Guid? eventSectorId, long? seatId);
    }
}
