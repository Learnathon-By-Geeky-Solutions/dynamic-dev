using EasyTravel.Application.Factories;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class GuideService : IAdminGuideService
    {
        private IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IAdminAgencyService _agencyService;
        private readonly IGuideFactory _guideFactory;
        public GuideService(IApplicationUnitOfWork applicationUnitOfWork,IAdminAgencyService agencyService,IGuideFactory guideFactory)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _agencyService = agencyService;
            _guideFactory = guideFactory;
        }
        public Guide GetGuideInstance()
        {
            var agencyList = _agencyService.GetAll();
            var guide = _guideFactory.CreateInstance();
            guide.Agencies = agencyList.ToList();
            return guide;
        }

        public void Create(Guide entity)
        {
            _applicationUnitOfWork.GuideRepository.Add(entity);
            _applicationUnitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            _applicationUnitOfWork.GuideRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Guide Get(Guid id)
        {
            return _applicationUnitOfWork.GuideRepository.GetById(id);
        }

        public IEnumerable<Guide> GetAll()
        {
            return _applicationUnitOfWork.GuideRepository.GetAll();
        }
        public void Update(Guide entity)
        {
            _applicationUnitOfWork.GuideRepository.Edit(entity);
            _applicationUnitOfWork.Save();
        }
    }
}
