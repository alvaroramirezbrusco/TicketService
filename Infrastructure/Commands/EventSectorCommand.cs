using Application.Interfaces.IEvenSeat;
using Application.Interfaces.IEventSector;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class EventSectorCommand : IEventSectorCommand
    {
        private readonly AppDbContext _context;

        public EventSectorCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteSector(EventSector sector)
        {
            _context.EventSectors.Remove(sector);
            await _context.SaveChangesAsync();
        }

        public async Task InsertSector(EventSector sector)
        {
            _context.EventSectors.Add(sector);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSector(EventSector sector)
        {
            _context.EventSectors.Update(sector);
            await _context.SaveChangesAsync();
        }
    }
}
