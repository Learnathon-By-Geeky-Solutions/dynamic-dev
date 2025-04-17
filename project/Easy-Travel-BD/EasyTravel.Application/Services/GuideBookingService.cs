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
    public class GuideBookingService : IGuideBookingService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GuideBookingService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddBooking(GuideBooking model,Booking booking)
        {
            _applicationUnitOfWork.GuideBookingRepository.Add(model);
            _applicationUnitOfWork.Save();
            booking.Id = model.Id;
            _applicationUnitOfWork.BookingRepository.Add(booking);
            _applicationUnitOfWork.Save();
        }

        public bool CancelBooking()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GuideBooking>> GetBookingListByFormDataAsync(GuideBooking model)
        {
            var difference = DateTime.Now.TimeOfDay - model.StartTime;
            if (difference < TimeSpan.FromHours(3))
            {
                return new List<GuideBooking>();
            }
            return await _applicationUnitOfWork.GuideBookingRepository.GetAsync(e =>
                    e.EventDate == model.EventDate &&
                    e.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                    (e.Booking.BookingStatus == BookingStatus.Confirmed || e.Booking.BookingStatus == BookingStatus.Pending) &&
                    (
                        (e.StartTime >= model.StartTime && e.StartTime <= model.EndTime) ||
                        (e.EndTime >= model.StartTime && e.EndTime <= model.EndTime)
                    )
                );

        }

        public async Task<bool> IsBooked(GuideBooking model)
        {
            var bookings = await GetBookingListByFormDataAsync(model);
            if (bookings.Any(e => e.GuideId == model.GuideId))
            {
                return true;
            }
            return false;
        }
    }
}
