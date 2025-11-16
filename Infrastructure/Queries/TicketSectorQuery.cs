using Application.Interfaces.ITicketSector;
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
    public class TicketSectorQuery : ITicketSectorQuery
    {
        private readonly AppDbContext _context;

        public TicketSectorQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TicketSector?> GetTicketSectorById(Guid id)
        {
            return await _context.TicketSectors
                .Include(ts => ts.EventSectorRef)
                .FirstOrDefaultAsync(ts => ts.TicketSectorId == id);
        }

        public async Task<List<TicketSector>> GetTicketSectorsByTicket(Guid ticketId)
        {
            return await _context.TicketSectors
                .Include(ts => ts.EventSectorRef)
                .Where(ts => ts.TicketId == ticketId)
                .ToListAsync();
        }
    }
}
