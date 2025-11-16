using Application.Interfaces.IEvenSeat;
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
    public class EventSeatQuery : IEventSeatQuery
    {
        private readonly AppDbContext _context;

        public EventSeatQuery(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<EventSeat?> GetEventSeatById(Guid id)
        {
            return await _context.EventSeats
                .Include(es => es.StatusRef)
                .FirstOrDefaultAsync(es => es.EventSeatId == id);
        }

        public async Task<List<EventSeat>> GetEventSeatsAllAsync()
        {
            return await _context.EventSeats
                .Include(es => es.StatusRef)
                .ToListAsync();
        }

        public async Task<EventSeat> GetEventSeatByEventSectorIdAsync(Guid? eventId, Guid? eventSectorId, long? seatId)
        {
            var query = _context.EventSeats
                .Include(ES => ES.StatusRef)
                .AsQueryable();
            if (eventId.HasValue)
            {
                query = query.Where(ES => ES.EventId == eventId);
            }
            if (eventSectorId.HasValue)
            {
                query = query.Where(ES => ES.EventSectorId == eventSectorId);
            }
            if (seatId.HasValue)
            {
                query = query.Where(ES => ES.SeatId == seatId);
            }
            return await query.FirstOrDefaultAsync();
        }      
    }
}
