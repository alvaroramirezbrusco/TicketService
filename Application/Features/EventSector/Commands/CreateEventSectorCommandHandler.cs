using Application.Interfaces.IEvenSeat;
using Application.Interfaces.IEventSector;
using Application.Models.Responses;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EventSector.Commands
{
    public class CreateEventSectorCommandHandler : IRequestHandler<CreateEventSectorCommand, List<EventSectorResponse>>
    {
        private readonly IEventSectorCommand _eventSectorCommand;
        private readonly IEventSectorQuery _eventSectorQuery;
        private readonly IEventSeatCommand _eventSeatCommand;
        private readonly IEventSeatQuery _eventSeatQuery;

        public CreateEventSectorCommandHandler(
            IEventSectorCommand eventSectorCommand,
            IEventSeatCommand eventSeatCommand,
            IEventSeatQuery eventSeatQuery,
            IEventSectorQuery eventSectorQuery)
        {
            _eventSectorCommand = eventSectorCommand;
            _eventSeatCommand = eventSeatCommand;
            _eventSeatQuery = eventSeatQuery;
            _eventSectorQuery = eventSectorQuery;
        }

        public async Task<List<EventSectorResponse>> Handle(CreateEventSectorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Request;

            
            if (dto.EventId == Guid.Empty)
            {
                throw new ArgumentException("Debe ingresar un ID de un Evento.");
            }                

            if (dto.Sectors == null || dto.Sectors.Count == 0)
            {
                throw new ArgumentException("El evento no tiene sectores asociados.");
            }                
                        
            var sectorsToCreate = new List<(Domain.Entities.EventSector Sector, List<Domain.Entities.EventSeat> Seats)>();

            // sirve para detectar duplicados de SectorId dentro del request
            var sectorIdsInRequest = new HashSet<Guid>();
                        
            foreach (var sectorDto in dto.Sectors)
            {
                
                if (!sectorIdsInRequest.Add(sectorDto.SectorId))
                {
                    throw new ArgumentException($"El sector {sectorDto.SectorId} está duplicado en el request.");
                }                 
                
                var sectorExist = await _eventSectorQuery.GetEventSectorByEventAndSectorAsync(dto.EventId, sectorDto.SectorId);
                if (sectorExist != null)
                {
                    throw new ArgumentException($"El sector {sectorDto.SectorId} ya está registrado para este evento.");
                }                    
                                
                if (sectorDto.Price <= 0)
                {
                    throw new ArgumentException($"El precio del sector {sectorDto.SectorId} debe ser mayor a 0.");
                }                    

                if (sectorDto.IsControlled)
                {                    
                    if (sectorDto.Seats == null || sectorDto.Seats.Count == 0)
                    {
                        throw new ArgumentException($"Un sector controlado debe tener asientos para poder ser creado.");
                    }                        

                    if (sectorDto.Seats.Any(seatId => seatId <= 0))
                    {
                        throw new ArgumentException($"Los asientos del sector {sectorDto.SectorId} deben tener un SeatId mayor a 0.");
                    }                        

                    var duplicatedSeats = sectorDto.Seats
                        .GroupBy(s => s)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();

                    if (duplicatedSeats.Any())
                    {
                        throw new ArgumentException($"El sector {sectorDto.SectorId} contiene asientos duplicados: {string.Join(", ", duplicatedSeats)}");
                    }                        
                }
                else
                {                    
                    if (!sectorDto.Capacity.HasValue || sectorDto.Capacity.Value <= 0)
                    {
                        throw new ArgumentException("Un sector no controlado debe tener una capacidad de personas mayor a 0.");
                    }                        
                }

                var eventSectorId = Guid.NewGuid();

                var sectorEntity = new Domain.Entities.EventSector
                {
                    EventSectorId = eventSectorId,
                    EventId = dto.EventId,
                    SectorId = sectorDto.SectorId,
                    Name = sectorDto.Name,
                    IsControlled = sectorDto.IsControlled,
                    Price = sectorDto.Price,
                    Capacity = sectorDto.IsControlled ? sectorDto.Seats.Count : sectorDto.Capacity!.Value,
                    SoldCount = 0,
                    ReservedCount = 0
                };

                var seatsEntities = new List<Domain.Entities.EventSeat>();

                if (sectorDto.IsControlled)
                {
                    foreach (var seatId in sectorDto.Seats)
                    {
                        var seatEntity = new Domain.Entities.EventSeat
                        {
                            EventSeatId = Guid.NewGuid(),
                            EventSectorId = eventSectorId,
                            EventId = dto.EventId,
                            SeatId = seatId,
                            Price = sectorDto.Price,
                            StatusId = 1 // Inicia disponible
                        };

                        seatsEntities.Add(seatEntity);
                    }
                }

                sectorsToCreate.Add((sectorEntity, seatsEntities));
            }

            var responses = new List<EventSectorResponse>();

            foreach (var (sector, seats) in sectorsToCreate)
            {
                await _eventSectorCommand.InsertSector(sector);

                if (sector.IsControlled && seats.Any())
                {
                    await _eventSeatCommand.InsertEventSeatRangeAsync(seats);
                }

                var response = new EventSectorResponse
                {
                    EventSectorId = sector.EventSectorId,
                    EventId = sector.EventId,
                    SectorId = sector.SectorId,
                    Name = sector.Name,
                    IsControlled = sector.IsControlled,
                    Capacity = sector.Capacity,
                    Price = sector.Price,
                    EventSeats = seats.Select(s => new EventSeatCreatedResponse
                    {
                       EventSeatId = s.EventSeatId,
                       SeatId = s.SeatId
                    }).ToList()
                };
                responses.Add(response);
            }
            return responses;
        }
    }
}
