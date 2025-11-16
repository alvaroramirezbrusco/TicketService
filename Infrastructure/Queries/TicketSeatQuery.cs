using Application.Interfaces.ITicketSeat;
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
    public class TicketSeatQuery : ITicketSeatQuery
    {
        private readonly AppDbContext _context;

        public TicketSeatQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TicketSeat?> GetTicketSeatById(Guid id)
        {
            return await _context.TicketSeats
                .Include(ts => ts.EventSeatRef)
                .FirstOrDefaultAsync(ts => ts.TicketSeatId == id);
        }

        public async Task<List<TicketSeat>> GetTicketSeatsByTicketId(Guid ticketId)
        {
            return await _context.TicketSeats
                .Include(ts => ts.EventSeatRef)
                .Where(ts => ts.TicketId == ticketId)
                .ToListAsync();
        }
    }
}
