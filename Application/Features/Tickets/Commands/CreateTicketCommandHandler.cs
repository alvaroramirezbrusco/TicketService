using Application.Interfaces.IEvenSeat;
using Application.Interfaces.IEventSector;
using Application.Interfaces.ITicket;
using Application.Interfaces.ITicketSeat;
using Application.Interfaces.ITicketSector;
using Application.Interfaces.ITicketStatus;
using Application.Models.Responses;
using Domain.Entities;
using MediatR;
using System.Data;

namespace Application.Features.Tickets.Commands
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketResponse>
    {
        private readonly ITicketCommand _ticketCommand;
        private readonly ITicketQuery _ticketQuery;
        private readonly IEventSeatCommand _eventSeatCommand;
        private readonly IEventSeatQuery _eventSeatQuery;
        private readonly ITicketStatusQuery _ticketStatusQuery;
        private readonly IEventSectorCommand _eventSectorCommand;
        private readonly IEventSectorQuery _eventSectorQuery;
        private readonly ITicketSeatCommand _ticketSeatCommand;
        private readonly ITicketSeatQuery _ticketSeatQuery;
        private readonly ITicketSectorCommand _ticketSectorCommand;
        private readonly ITicketSectorQuery _ticketSectorQuery;

        public CreateTicketCommandHandler(ITicketCommand command, IEventSeatCommand eventCommand, IEventSeatQuery eventSeatQuery,
            ITicketStatusQuery ticketStatusQuery, ITicketQuery ticketQuery, IEventSectorQuery eventSectorQuery, IEventSectorCommand eventSectorCommand, 
            ITicketSectorQuery ticketSectorQuery, ITicketSectorCommand ticketSectorCommand, ITicketSeatQuery ticketSeatQuery, ITicketSeatCommand ticketSeatCommand)
        {
            _ticketCommand = command;
            _eventSeatCommand = eventCommand;
            _eventSeatQuery = eventSeatQuery;
            _ticketStatusQuery = ticketStatusQuery;
            _ticketQuery = ticketQuery;
            _eventSectorQuery = eventSectorQuery;
            _eventSectorCommand = eventSectorCommand;
            _ticketSectorQuery = ticketSectorQuery;
            _ticketSectorCommand = ticketSectorCommand;
            _ticketSeatQuery = ticketSeatQuery;
            _ticketSeatCommand = ticketSeatCommand;

        }

        public async Task<TicketResponse> Handle(CreateTicketCommand Request, CancellationToken cancellationToken)
        {
            var dto = Request.Request;            
            if (dto.UserId == Guid.Empty)
            {
                throw new ArgumentException("Debe ingresar un Id de usuario");
            }
            if (dto.EventId == Guid.Empty)
            {
                throw new ArgumentException("Debe ingresar el Id del evento");
            }
            if ((dto.EventSeatIds == null || dto.EventSeatIds.Count == 0) && (dto.Sectors == null || dto.Sectors.Count == 0))
            {
                throw new ArgumentException("Debe seleccionar plateas o sectores");
            }
            
            var statusReferencia = await _ticketStatusQuery.GetTicketStatusById(1);
            if(statusReferencia == null)
            {
                throw new ArgumentException("No se encontro el estado con Id 1 para iniciar un ticket como disponible");
            }
            
            // validar los asientos o lugares en campo antes de crear el ticket 
            var seatsToUpdate = new List<Domain.Entities.EventSeat>();        
            if (dto.EventSeatIds != null)
            {
                foreach (var seatId in dto.EventSeatIds)
                {
                    var seat = await _eventSeatQuery.GetEventSeatById(seatId);
                    if (seat == null)
                    {
                        throw new ArgumentException($"Seat: {seatId} no existe.");
                    }
                    if (seat.StatusId == 3)
                    {
                        throw new ArgumentException($"Seat {seat.EventSeatId} ya esta vendido");
                    }
                    if (seat.StatusId == 2 && seat.ReservedByUserId != dto.UserId)
                    {
                        throw new ArgumentException($"Seat {seat.EventSeatId} esta reservado por otro usuario.");
                    }
                    seatsToUpdate.Add(seat);
                }
            }
            var EventSectorsReserved = new List<(Domain.Entities.EventSector sector, int quantity)>();
            if (dto.Sectors != null)
            {
                foreach (var sectorDto in dto.Sectors)
                {
                    var sector = await _eventSectorQuery.GetEventSectorByIdAsync(sectorDto.EventSectorId);
                    if (sector == null)
                    {
                        throw new ArgumentException($"El sector {sectorDto.EventSectorId} no existe para el evento {dto.EventId}");
                    }
                    if (sector.IsControlled)
                    {
                        throw new ArgumentException("Sector controlado requiere lista de asientos a comprar");
                    }
                    if (sectorDto.Quantity <= 0)
                    {
                        throw new ArgumentException("Debe ingresar una cantidad valida mayor a cero.");
                    }
                    if (sector.SoldCount + sector.ReservedCount + sectorDto.Quantity > sector.Capacity)
                    {
                        throw new ArgumentException("No hay capacidad disponible en el sector para crear un ticket");
                    }
                    EventSectorsReserved.Add((sector, sectorDto.Quantity));
                }
            }

            // crear el ticket
            var ticket = new Ticket
            {
                TicketId = Guid.NewGuid(),
                UserId = dto.UserId,
                EventId = dto.EventId,
                StatusId = 1, // inicia como disponible
                StatusRef = statusReferencia,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
            };
            await _ticketCommand.InsertTicket(ticket);

            // creamos ticketSeat o ticketSector
            foreach (var eventSeat in seatsToUpdate)
            {
                var ticketSeat = new TicketSeat
                {
                    TicketSeatId = Guid.NewGuid(),
                    TicketId = ticket.TicketId,
                    EventSeatId = eventSeat.EventSeatId
                };
                await _ticketSeatCommand.InsertTicketSeat(ticketSeat);
                eventSeat.StatusId = 3;
                eventSeat.ReservedByUserId = null;
                await _eventSeatCommand.UpdateEventSeat(eventSeat);
            }

            foreach (var (sector,quantity) in EventSectorsReserved)
            {
                var ticketSector = new TicketSector
                {
                    TicketSectorId = Guid.NewGuid(),
                    TicketId = ticket.TicketId,
                    EventSectorId = sector.EventSectorId,
                    Quantity = quantity
                };
                await _ticketSectorCommand.InsertTicketSector(ticketSector);
                sector.SoldCount += quantity;
                if (sector.ReservedCount > 0)
                {                    
                    sector.ReservedCount -= Math.Min(quantity, sector.ReservedCount);
                }
                await _eventSectorCommand.UpdateSector(sector);
            }

            var saveTicket = await _ticketQuery.GetTicketById(ticket.TicketId);
            return new TicketResponse
            {
                TicketId = saveTicket.TicketId,
                UserId = saveTicket.UserId,
                EventId = saveTicket.EventId,
                Created = saveTicket.Created,
                Updated = saveTicket.Updated,
                Status = new TicketStatusResponse
                {
                    StatusID = saveTicket.StatusRef.StatusID,
                    Name = saveTicket.StatusRef.Name,
                },
                TicketSeats = saveTicket.TicketSeats.Select(ts => new TicketSeatResponse
                {
                    TicketSeatId = ts.TicketSeatId,
                    TicketId = ts.TicketId,
                    EventSeatId = ts.EventSeatId,
                }).ToList(),
                TicketSectors = saveTicket.TicketSectors.Select(ts => new TicketSectorResponse
                {
                    TicketSectorId = ts.TicketSectorId,
                    TicketId = ts.TicketId,
                    Quantity = ts.Quantity
                }).ToList()
            };
        }
    }
}
