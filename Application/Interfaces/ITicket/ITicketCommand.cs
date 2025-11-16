using Domain.Entities;


namespace Application.Interfaces.ITicket
{
    public interface ITicketCommand
    {
        Task InsertTicket(Ticket ticket);
        Task UpdateTicket(Ticket ticket);
    }
}
