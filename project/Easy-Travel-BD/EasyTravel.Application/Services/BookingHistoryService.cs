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
    public class BookingHistoryService : IBookingHistoryService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public BookingHistoryService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public async Task<IEnumerable<BusBooking>> GetBusBookingsAsync(Guid id)
        {
            throw new NotImplementedException();
            //return await _applicationUnitOfWork.BusBookingRepository.GetAsync(e => e.UserId == id);
        }

        public async Task<IEnumerable<CarBooking>> GetCarBookingsAsync(Guid id)
        {
            throw new NotImplementedException();
            //return await _applicationUnitOfWork.CarBookingRepository.GetAsync(e => e.UserId == id);
        }

        public async Task<IEnumerable<GuideBooking>> GetGuideBookingsAsync(Guid id)
        {
            throw new NotImplementedException();
            //return await _applicationUnitOfWork.GuideBookingRepository.GetAsync(e => e.UserId == id);
        }

        public async Task<IEnumerable<HotelBooking>> GetHotelBookingsAsync(Guid id)
        {
            throw new NotImplementedException();
            //return await _applicationUnitOfWork.HotelBookingRepository.GetAsync(e => e.UserId == id);
        }

        public async Task<IEnumerable<PhotographerBooking>> GetPhotographerBookingsAsync(Guid id)
        {
            throw new NotImplementedException();
            //return await _applicationUnitOfWork.PhotographerBookingRepository.GetAsync(e => e.UserId == id);
        }
    }
}
