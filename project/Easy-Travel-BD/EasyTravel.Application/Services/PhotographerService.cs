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
            throw new NotImplementedException();
        }

        public IEnumerable<Photographer> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Photographer>> GetPhotographerListAsync(PhotographerBooking photographerBooking)
        {
            var bookings =  await _photographerBookingService.GetBookingListAsync(photographerBooking);
            if (bookings == null)
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
