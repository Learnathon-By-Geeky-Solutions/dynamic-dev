using EasyTravel.Domain;
using EasyTravel.Domain.Repositories;
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
        public ApplicationUnitOfWork(ApplicationDbContext context,IUserRepository userRepository,IBusRepository busRepository) 
           : base(context)
        {


            UserRepository = userRepository;
            BusRepository = busRepository;
        }

        public IUserRepository UserRepository { get; private set; }
        public IBusRepository BusRepository { get; private set; }
    }
}
