using Application.Interfaces.ITicketSeat;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class TicketSeatCommand : ITicketSeatCommand
    {
        private readonly AppDbContext _context;

        public TicketSeatCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertTicketSeat(TicketSeat ticketSeat)
        {
            _context.TicketSeats.Add(ticketSeat);
            await _context.SaveChangesAsync();
        }

        public async Task InsertTicketSeatRange(IEnumerable<TicketSeat> ticketSeats)
        {
            _context.TicketSeats.AddRange(ticketSeats);
            await _context.SaveChangesAsync();
        }
    }
}
