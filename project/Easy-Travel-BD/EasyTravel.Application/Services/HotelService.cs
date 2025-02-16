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
        private readonly IApplicationUnitOfWork _applicationunitOfWork;

        public HotelService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationunitOfWork = applicationUnitOfWork;
        }



        public void Create(Hotel entity)
        {
            _applicationunitOfWork.HotelRepository.Add(entity);
            _applicationunitOfWork.Save();

        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Hotel Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hotel> GetAll()
        {
            return _applicationunitOfWork.HotelRepository.GetAll();
        }

        public void Update(Hotel entity)
        {
            throw new NotImplementedException();
        }
    }
}
