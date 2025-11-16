using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ITicketSector
{
    public interface ITicketSectorQuery
    {
        Task<TicketSector?> GetTicketSectorById(Guid id);
        Task<List<TicketSector>> GetTicketSectorsByTicket(Guid ticketId);
    }
}
