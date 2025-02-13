using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{

    public class HotelService : IHotelService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public HotelService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;

        }
        public void CreateHotel(Hotel hotel)
        {
            _applicationUnitOfWork.HotelRepository.AddHotel(hotel);
            _applicationUnitOfWork.Save();
        }

        public void DeleteHotel(Hotel hotel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            throw new NotImplementedException();
        }

        public Hotel GetHotelById(Guid HotelId)
        {
            throw new NotImplementedException();
        }

        public void UpdateHotel(Hotel hotel)
        {
            throw new NotImplementedException();
        }
    }
}
