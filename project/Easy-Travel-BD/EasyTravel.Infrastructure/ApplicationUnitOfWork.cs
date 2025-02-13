using EasyTravel.Domain;
using EasyTravel.Domain.Repositories;
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
        public ApplicationUnitOfWork(ApplicationDbContext context,IUserRepository userRepository,IBusRepository busRepository ,IHotelRepository hotelRepository ) 
           : base(context)
        {


            UserRepository = userRepository;
            BusRepository = busRepository;
            HotelRepository = hotelRepository;
        }

        public IUserRepository UserRepository { get; private set; }
        public IBusRepository BusRepository { get; private set; }
        public IHotelRepository HotelRepository { get; private set; }

       
    }
}
