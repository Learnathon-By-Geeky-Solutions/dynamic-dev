using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain
{
    public interface IApplicationUnitOfWork: IUnitOfWork
    {

        public IUserRepository UserRepository { get; }
        public IBusRepository BusRepository { get; }
        public IAgencyRepository AgencyRepository { get; }
        public IPhotographerRepository PhotographerRepository { get; }
        public IGuideRepository GuideRepository { get; }
        public IHotelRepository HotelRepository { get; }
        public IRoomRepository RoomRepository { get; }
        public IHotelBookingRepository HotelBookingRepository { get; }
        public ICarRepository CarRepository { get; }
        
    }
}
