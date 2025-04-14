using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IHotelService : IService<Hotel, Guid>
    {
        IEnumerable<Hotel> SearchHotels(string location, DateTime? value);
    }
}
