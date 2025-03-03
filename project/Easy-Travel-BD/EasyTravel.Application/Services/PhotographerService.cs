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
    public class PhotographerService : IPhotographerService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public PhotographerService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public Photographer Get(Guid id)
        {
            return _applicationUnitOfWork.PhotographerRepository.GetById(id);
        }

        public IEnumerable<Photographer> GetAll()
        {
            return _applicationUnitOfWork.PhotographerRepository.GetAll();
        }
    }
}
