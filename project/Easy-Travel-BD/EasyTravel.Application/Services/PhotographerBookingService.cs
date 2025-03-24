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
        public void AddBooking(PhotographerBooking model)
        {
            _applicationUnitOfWork.PhotographerBookingRepository.Add(model);
            _applicationUnitOfWork.Save();
        }

        public bool CancelBooking()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PhotographerBooking>> GetBookingListByFormDataAsync(PhotographerBooking model)
        {
            return await _applicationUnitOfWork.PhotographerBookingRepository.GetAsync(e =>
            e.EventDate == model.EventDate && e.StartTime > DateTime.Now.TimeOfDay && e.StartTime >= model.StartTime && e.StartTime <= model.EndTime || e.EndTime >= model.StartTime && e.EndTime <= model.EndTime);

        }

        public async Task<bool> IsBooked(PhotographerBooking model,Guid id)
        {
            var bookings = await GetBookingListByFormDataAsync(model);
            if (bookings.Any(e => e.PhotographerId == id))
            {
                return true;
            }
            return false;
        }
    }
}
