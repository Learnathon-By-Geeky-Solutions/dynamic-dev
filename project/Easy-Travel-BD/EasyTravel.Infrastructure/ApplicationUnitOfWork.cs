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
        public ApplicationUnitOfWork(ApplicationDbContext context,IUserRepository userRepository,IBusRepository busRepository,IAgencyRepository agencyRepository,IPhotographerRepository photographerRepository,IGuideRepository guideRepository) 
           : base(context)
        {
            UserRepository = userRepository;
            BusRepository = busRepository;
            AgencyRepository = agencyRepository;
            PhotographerRepository = photographerRepository;
            GuideRepository = guideRepository;
        }

        public IUserRepository UserRepository { get; private set; }
        public IBusRepository BusRepository { get; private set; }
        public IAgencyRepository AgencyRepository { get; private set; }
        public IPhotographerRepository PhotographerRepository { get;private set; }
        public IGuideRepository GuideRepository { get; private set; }
    }
}
