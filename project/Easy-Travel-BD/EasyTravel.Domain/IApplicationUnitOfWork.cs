using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain
{
    public interface IApplicationUnitOfWork: IUnityOfWork
    {

        public IUserRepository UserRepository { get; }
        public IBusRepository BusRepository { get; }

        public IHotelRepository HotelRepository { get; }

        
    }
}
