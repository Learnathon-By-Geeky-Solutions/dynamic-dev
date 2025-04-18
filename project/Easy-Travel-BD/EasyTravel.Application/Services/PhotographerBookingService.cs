using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class PhotographerBookingService : IPhotographerBookingService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public PhotographerBookingService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddBooking(PhotographerBooking model, Booking booking)
        {
            _applicationUnitOfWork.BookingRepository.Add(booking);
            _applicationUnitOfWork.Save();
            model.Id = booking.Id;
            _applicationUnitOfWork.PhotographerBookingRepository.Add(model);
            _applicationUnitOfWork.Save();
        }

        public bool CancelBooking()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PhotographerBooking>> GetBookingListByFormDataAsync(PhotographerBooking model)
        {
            var difference = DateTime.Now.TimeOfDay - model.StartTime;
            if (difference < TimeSpan.FromHours(6))
            {
                return new List<PhotographerBooking>();
            }

            return await _applicationUnitOfWork.PhotographerBookingRepository.GetAsync(e =>
                 e.EventDate >= model.EventDate &&
                 e.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                 (e.Booking.BookingStatus == BookingStatus.Confirmed || e.Booking.BookingStatus == BookingStatus.Pending) &&
                 (
                     (e.StartTime >= model.StartTime && e.StartTime <= model.EndTime) ||
                     (e.EndTime >= model.StartTime && e.EndTime <= model.EndTime)
                 )
             );

        }

        public async Task<bool> IsBooked(PhotographerBooking model)
        {
            var bookings = await GetBookingListByFormDataAsync(model);
            if (bookings.Any(e => e.PhotographerId == model.PhotographerId))
            {
                return true;
            }
            return false;
        }
    }
}
