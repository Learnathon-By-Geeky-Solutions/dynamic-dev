using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class PhotographerService : IPhotographerService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public PhotographerService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
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
            DateTime now = DateTime.Now;
            DateTime selectedDateTime = photographerBooking.EventDate.Add(photographerBooking.StartTime);
            TimeSpan difference = selectedDateTime - now;
            if (selectedDateTime < now || difference < TimeSpan.FromHours(6))
            {
                return new List<Photographer>(); 
            }

            return await _applicationUnitOfWork.PhotographerRepository.GetAsync(
                e => e.Availability &&
                    !e.PhotographerBookings!.Any() ||
                     e.PhotographerBookings!.Any(
                         p => p.Booking!.BookingStatus != BookingStatus.Confirmed && p.Booking.BookingStatus != BookingStatus.Pending &&
                             p.EventDate >= photographerBooking.EventDate &&
                              p.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                 (
                     (p.StartTime >= photographerBooking.StartTime && p.EventDate >= photographerBooking.EventDate) ||
                     (p.EndTime >= photographerBooking.StartTime && p.EventDate >= photographerBooking.EventDate)
                 )
                            )
            );
        }
    }
}
