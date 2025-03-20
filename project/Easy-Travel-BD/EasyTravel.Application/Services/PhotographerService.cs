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
    public class PhotographerService : IPhotographerService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IPhotographerBookingService _photographerBookingService;
        public PhotographerService(IApplicationUnitOfWork applicationUnitOfWork, IPhotographerBookingService photographerBookingService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _photographerBookingService = photographerBookingService;
        }

        public Photographer Get(Guid id)
        {
            return _applicationUnitOfWork.PhotographerRepository.GetById(id);
        }

        public IEnumerable<Photographer> GetAll()
        {
            return _applicationUnitOfWork.PhotographerRepository.GetAll();
        }

        public async Task<IEnumerable<Photographer>> GetPhotographerListAsync(PhotographerBooking photographerBooking)
        {
            var bookings =  await _photographerBookingService.GetBookingListAsync(photographerBooking);
            if (bookings.ToList().Count() == 0)
            {
                return await _applicationUnitOfWork.PhotographerRepository.GetAsync(e => e.Availability == true);
            }
            var pglist = new List<Photographer>();
            foreach(var booking in bookings)
            {
                var pgmodel = await _applicationUnitOfWork.PhotographerRepository.GetByIdAsync(booking.Id);
                pglist.Add(pgmodel);
            }
            return pglist;
        }
    }
}
