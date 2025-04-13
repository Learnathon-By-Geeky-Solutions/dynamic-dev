using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Repositories
{
    public interface IHotelRepository : IRepository<Hotel, Guid>
    {
        IEnumerable<Hotel> GetHotels(string location, DateTime travelDateTime);
    }
}
