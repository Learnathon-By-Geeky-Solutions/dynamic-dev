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
            var difference = guideBooking.StartTime -  DateTime.Now.TimeOfDay;
            if (difference < TimeSpan.FromHours(6))
            {
                return new List<Guide>();
            }
            return await _applicationUnitOfWork.GuideRepository.GetAsync(
                e => e.Availability &&
                    !e.GuideBookings.Any() ||
                     e.GuideBookings.Any(
                         p => p.Booking.BookingStatus != BookingStatus.Confirmed && p.Booking.BookingStatus != BookingStatus.Pending &&
                             p.EventDate >= guideBooking.EventDate &&
                              p.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                 (
                     (p.StartTime >= guideBooking.StartTime && p.EventDate >= guideBooking.EventDate) ||
                     (p.EndTime >= guideBooking.StartTime && p.EventDate >= guideBooking.EventDate)
                 )
                            )
            );
        }
    }
}
