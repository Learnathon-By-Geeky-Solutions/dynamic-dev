using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IHotelService : IService<Hotel, Guid>
    {
        public PagedResult<Hotel> GetAllPaginatedHotels(int pageNumber, int pageSize);
        PagedResult<Hotel> SearchHotels(string location, DateTime? value, int pageNumber, int pageSize);
    }
}
