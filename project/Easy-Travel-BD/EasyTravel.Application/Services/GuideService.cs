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
    public class GuideService : IGuideService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GuideService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public Guide Get(Guid id)
        {
            return _applicationUnitOfWork.GuideRepository.GetById(id);
        }

        public IEnumerable<Guide> GetAll()
        {
            return _applicationUnitOfWork.GuideRepository.GetAll();
        }
    }
}
