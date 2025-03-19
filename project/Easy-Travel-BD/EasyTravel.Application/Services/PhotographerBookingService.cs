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
    public class PhotographerBookingService : IPhotographerBookingService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public PhotographerBookingService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddBooking()
        {
            throw new NotImplementedException();
        }

        public bool CancelBooking()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PhotographerBooking>> GetBookingListAsync(PhotographerBooking photographerBooking)
        {
            return await _applicationUnitOfWork.PhotographerBookingRepository.GetAsync(e =>
            e.EventDate == photographerBooking.EventDate && (e.EndTime < photographerBooking.StartTime || e.StartTime > photographerBooking.EndTime));

        }
    }
}
