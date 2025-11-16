using Application.Interfaces.IEventSector;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class EventSectorQuery : IEventSectorQuery
    {
        private readonly AppDbContext _context;

        public EventSectorQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EventSector?> GetEventSectorByEventAndSectorAsync(Guid eventId, Guid SectorId)
        {
            return await _context.EventSectors.FirstOrDefaultAsync(es => es.EventId == eventId && es.SectorId == SectorId);
        }

        public async Task<List<EventSector>> GetEventSectorByEventIdAsync(Guid eventId)
        {
           return await _context.EventSectors
                .Where(s => s.EventId == eventId).ToListAsync();
        }

        public async Task<EventSector?> GetEventSectorByIdAsync(Guid eventSectorId)
        {
            return await _context.EventSectors.FirstOrDefaultAsync(es => es.EventSectorId == eventSectorId);
        }

        public async Task<EventSector?> GetEventSectorBySectorId(Guid sectorId)
        {
            return await _context.EventSectors.FirstOrDefaultAsync(es => es.SectorId == sectorId);
        }
    }
}
