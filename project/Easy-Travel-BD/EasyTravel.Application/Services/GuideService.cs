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
    public class GuideService : IGuideService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IGuideBookingService _guideBookingService;
        public GuideService(IApplicationUnitOfWork applicationUnitOfWork, IGuideBookingService guideBookingService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _guideBookingService = guideBookingService;
        }
        public Guide Get(Guid id)
        {
            return _applicationUnitOfWork.GuideRepository.GetById(id);
        }

        public IEnumerable<Guide> GetAll()
        {
            return _applicationUnitOfWork.GuideRepository.GetAll();
        }

        public async Task<IEnumerable<Guide>> GetGuideListAsync(GuideBooking guideBooking)
        {
            var bookings = await _guideBookingService.GetBookingListAsync(guideBooking);
            if (bookings.ToList().Count() == 0)
            {
                return await _applicationUnitOfWork.GuideRepository.GetAsync(e => e.Availability == true);
            }
            var guidelist = new List<Guide>();
            foreach (var booking in bookings)
            {
                var guidemodel = await _applicationUnitOfWork.GuideRepository.GetByIdAsync(booking.Id);
                guidelist.Add(guidemodel);
            }
            return guidelist;
        }
    }
}
