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

        public async Task<IEnumerable<PhotographerBooking>> GetBookingListAsync(PhotographerBooking model)
        {
            return await _applicationUnitOfWork.PhotographerBookingRepository.GetAsync(e =>
             e.EventDate.Date >= model.EventDate.Date && (e.EndTime < model.StartTime || e.StartTime > model.EndTime));

        }
    }
}
