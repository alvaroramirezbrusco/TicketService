using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IEventSector
{
    public interface IEventSectorCommand
    {
        Task InsertSector(EventSector sector);
        Task UpdateSector(EventSector sector);
        Task DeleteSector(EventSector sector);
    }
}
