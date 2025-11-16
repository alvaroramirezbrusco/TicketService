using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ITicketSeat
{
    public interface ITicketSeatQuery
    {
        Task<TicketSeat?> GetTicketSeatById(Guid id);
        Task<List<TicketSeat>> GetTicketSeatsByTicketId(Guid ticketId);
    }
}
