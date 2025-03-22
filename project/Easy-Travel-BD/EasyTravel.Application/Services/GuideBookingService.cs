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
        public void AddBooking(GuideBooking model)
        {
            _applicationUnitOfWork.GuideBookingRepository.Add(model);
            _applicationUnitOfWork.Save();
        }

        public bool CancelBooking()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GuideBooking>> GetBookingListAsync(GuideBooking guideBooking)
        {
            return await _applicationUnitOfWork.GuideBookingRepository.GetAsync(e =>
            e.EventDate == guideBooking.EventDate && (e.EndTime < guideBooking.StartTime || e.StartTime > guideBooking.EndTime));
        }
    }
}
