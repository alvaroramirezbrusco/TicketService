using Application.Interfaces.ITicket;
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
    public class TicketQuery : ITicketQuery
    {
        private readonly AppDbContext _context;

        public TicketQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistingTicketById(Guid ticketId)
        {
            var exist = await _context.Tickets.AnyAsync(t => t.TicketId == ticketId);
            if (!exist)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Ticket>> GetTicketAllAsync(Guid? eventId, Guid? UserId)
        {
            var query=  _context.Tickets
                .Include(t => t.StatusRef)
                .Include(t => t.TicketSeats)
                    .ThenInclude(ts => ts.EventSeatRef)
                .Include(t => t.TicketSectors)
                    .ThenInclude(ts => ts.EventSectorRef)
                .AsQueryable();

            if (eventId.HasValue)
            {
                query = query.Where(t => t.EventId == eventId.Value);
            }
            if (UserId.HasValue)
            {
                query = query.Where(t => t.UserId == UserId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Ticket?> GetTicketById(Guid ticketID)
        {
            return await _context.Tickets
                .Include(t => t.StatusRef)
                .Include(t => t.TicketSeats)
                    .ThenInclude(ts => ts.EventSeatRef)
                .Include(t => t.TicketSectors)
                    .ThenInclude(ts => ts.EventSectorRef)
                .FirstOrDefaultAsync(t => t.TicketId == ticketID);
        }
    }
}
