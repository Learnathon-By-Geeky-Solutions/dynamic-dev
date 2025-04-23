using EasyTravel.Domain;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public ApplicationUnitOfWork(ApplicationDbContext context, IBusRepository busRepository, IAgencyRepository agencyRepository, IPhotographerRepository photographerRepository,IGuideRepository guideRepository, ICarRepository carRepository, IHotelRepository hotelRepository, IRoomRepository roomRepository, IHotelBookingRepository hotelBookingRepository,IBusBookingRepository busBookingRepository,ISeatRepository seatRepository,ICarBookingRepository carbookingRepository,IPhotographerBookingRepository photographerBookingRepository,IGuideBookingRepository guideBookingRepository, IBookingRepository bookingRepository)
           : base(context)
        {
            CarRepository = carRepository;
            BusRepository = busRepository;
            AgencyRepository = agencyRepository;
            PhotographerRepository = photographerRepository;
            GuideRepository = guideRepository;
            HotelRepository = hotelRepository;
            RoomRepository = roomRepository;
            HotelBookingRepository = hotelBookingRepository;
            PhotographerBookingRepository = photographerBookingRepository;
            GuideBookingRepository = guideBookingRepository;
            BusBookingRepository = busBookingRepository;
            SeatRepository = seatRepository;
            CarBookingRepository = carbookingRepository;
            BookingRepository = bookingRepository;
        }
        public IBusRepository BusRepository { get; private set; }
        public ICarRepository CarRepository { get; private set; }
        public IAgencyRepository AgencyRepository { get; private set; }
        public IPhotographerRepository PhotographerRepository { get;private set; }
        public IGuideRepository GuideRepository { get; private set; }
        public IHotelRepository HotelRepository { get; private set; }
        public IRoomRepository RoomRepository { get; private set; }
        public IHotelBookingRepository HotelBookingRepository { get; private set; }
        public IPhotographerBookingRepository PhotographerBookingRepository { get; private set; }
        public IGuideBookingRepository GuideBookingRepository { get; private set; }
        public IBusBookingRepository BusBookingRepository { get; private set; }
        public ISeatRepository SeatRepository { get; private set; }
        public ICarBookingRepository CarBookingRepository { get; private set; }
        public IBookingRepository BookingRepository{ get; private set; }
        public IPaymentRepository PaymentRepository { get; private set; }
    }
}
