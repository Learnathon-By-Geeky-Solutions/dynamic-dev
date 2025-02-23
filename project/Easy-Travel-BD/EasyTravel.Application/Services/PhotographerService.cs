using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Repositories;
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
        private readonly IAgencyService _agencyService;
        private readonly IEntityFactory<Photographer> _photographerFactory;
        public PhotographerService(IApplicationUnitOfWork applicationUnitOfWork, IAgencyService agencyService, IEntityFactory<Photographer> phototgrapherFactory)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _agencyService = agencyService;
            _photographerFactory = phototgrapherFactory;
        }
        public Photographer GetPhotographerInstance()
        {
            var agencyList = _agencyService.GetAll();
            var photographer = _photographerFactory.CreateInstance();
            photographer.Agencies = agencyList.ToList();
            return photographer;
        }

        public void Create(Photographer Photographer)
        {
            _applicationUnitOfWork.PhotographerRepository.Add(Photographer);
            _applicationUnitOfWork.Save();
        }

        public void Update(Photographer Photographer)
        {
            _applicationUnitOfWork.PhotographerRepository.Edit(Photographer);
            _applicationUnitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            _applicationUnitOfWork.PhotographerRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Photographer Get(Guid id)
        {
            var agencyList = _agencyService.GetAll();
            var model = _applicationUnitOfWork.PhotographerRepository.GetById(id);
            model.Agencies = agencyList.ToList();
            return model;
        }

        public IEnumerable<Photographer> GetAll()
        {
            return _applicationUnitOfWork.PhotographerRepository.GetAll();
        }
    }
}
