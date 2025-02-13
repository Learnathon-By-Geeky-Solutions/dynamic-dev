using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IHotelService
    {
        void CreateHotel(Hotel hotel);
        IEnumerable<Hotel> GetAllHotels();

        Hotel GetHotelById(Guid HotelId);
        void UpdateHotel(Hotel hotel);
        void DeleteHotel(Hotel hotel);
    }
}
