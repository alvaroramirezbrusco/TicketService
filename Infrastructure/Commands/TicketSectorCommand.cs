using Application.Interfaces.ITicketSector;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class TicketSectorCommand : ITicketSectorCommand
    {
        private readonly AppDbContext _context;

        public TicketSectorCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertTicketSector(TicketSector ticketSector)
        {
            _context.TicketSectors.Add(ticketSector);
            await _context.SaveChangesAsync();
        }

        public async Task InsertTicketSectorRange(IEnumerable<TicketSector> ticketSectors)
        {
            _context.TicketSectors.AddRange(ticketSectors);
            await _context.SaveChangesAsync();
        }
    }
}
