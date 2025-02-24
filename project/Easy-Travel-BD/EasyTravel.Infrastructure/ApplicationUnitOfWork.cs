using EasyTravel.Domain;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using EasyTravel.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {   
        public ApplicationUnitOfWork(ApplicationDbContext context, IUserRepository userRepository, IBusRepository busRepository, IAgencyRepository agencyRepository, IPhotographerRepository photographerRepository, ICarRepository carRepository, IHotelRepository hotelRepository, IRoomRepository roomRepository, IHotelBookingRepository hotelBookingRepository)
           : base(context)
        {
            UserRepository = userRepository;
            CarRepository = carRepository;
            BusRepository = busRepository;
            AgencyRepository = agencyRepository;
            PhotographerRepository = photographerRepository;
            HotelRepository = hotelRepository;
            RoomRepository = roomRepository;
            HotelBookingRepository = hotelBookingRepository;
        }

        public IUserRepository UserRepository { get; private set; }
        public IBusRepository BusRepository { get; private set; }
        public ICarRepository CarRepository { get; private set; }
        public IAgencyRepository AgencyRepository { get; private set; }
        public IPhotographerRepository PhotographerRepository { get;private set; }
        public IGuideRepository GuideRepository { get; private set; }
        public IHotelRepository HotelRepository { get; private set; }
        public IRoomRepository RoomRepository { get; private set; }
        public IHotelBookingRepository HotelBookingRepository { get; private set; }
    }
}
