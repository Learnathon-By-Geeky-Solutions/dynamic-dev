using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IRoomService:IService<Room,Guid>
    {
        Task<IEnumerable<Room>> GetRoomByHotel(Guid id);
        Task<PagedResult<Room>> GetPaginatedRoomsAsync(int pageNumber, int pageSize);
    }
}
