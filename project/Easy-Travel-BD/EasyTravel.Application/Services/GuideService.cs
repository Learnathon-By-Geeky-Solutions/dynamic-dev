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
            var bookings = await _applicationUnitOfWork.GuideBookingRepository.GetAsync(e =>
            e.EventDate == guideBooking.EventDate && e.StartTime > DateTime.Now.TimeOfDay && e.StartTime >= guideBooking.StartTime && e.StartTime <= guideBooking.EndTime || e.EndTime >= guideBooking.StartTime && e.EndTime <= guideBooking.EndTime);
            var guides = await _applicationUnitOfWork.GuideRepository.GetAsync(e => e.Availability == true);
            if (bookings.ToList().Count() == 0)
            {
                return guides;
            }
            var guideList = new List<Guide>();
            foreach (var guide in guides)
            {
                if (bookings.Any(e => e.GuideId != guide.Id))
                {
                    guideList.Add(guide);
                }
            }
            return guideList;
        }
    }
}
