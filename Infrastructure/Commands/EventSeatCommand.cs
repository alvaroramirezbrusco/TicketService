using Application.Interfaces.IEvenSeat;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Commands
{
    public class EventSeatCommand : IEventSeatCommand
    {
        private readonly AppDbContext _context;

        public EventSeatCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertEventSeat(EventSeat eventSeat)
        {
            _context.EventSeats.Add(eventSeat);
            await _context.SaveChangesAsync();
        }

        public async Task InsertEventSeatRangeAsync(IEnumerable<EventSeat> eventSeats)
        {
            await _context.EventSeats.AddRangeAsync(eventSeats);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventSeat(EventSeat evetSeat)
        {
            _context.EventSeats.Update(evetSeat);
            await _context.SaveChangesAsync();
        }
    }
}
