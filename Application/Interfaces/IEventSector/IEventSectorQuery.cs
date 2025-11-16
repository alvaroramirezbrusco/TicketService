using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IEventSector
{
    public interface IEventSectorQuery
    {
        Task<EventSector?> GetEventSectorByIdAsync(Guid eventSectorId);
        Task<List<EventSector>> GetEventSectorByEventIdAsync(Guid eventId);
        Task<EventSector?> GetEventSectorByEventAndSectorAsync(Guid eventId, Guid eventSectorId);
        Task<EventSector?> GetEventSectorBySectorId(Guid sectorId);
    }
}
