using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
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
        public PhotographerService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddPhotographer(Photographer Photographer)
        {
            _applicationUnitOfWork.PhotographerRepository.Add(Photographer);
            _applicationUnitOfWork.Save();
        }

        public void DeletePhotographer(Guid id)
        {
            _applicationUnitOfWork.PhotographerRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public IEnumerable<Photographer> GetAllPhotographers()
        {
            return _applicationUnitOfWork.PhotographerRepository.GetAll();
        }

        public Photographer GetPhotographerById(Guid id)
        {
            return _applicationUnitOfWork.PhotographerRepository.GetById(id);
        }

        public void UpdatePhotographer(Photographer Photographer)
        {
            _applicationUnitOfWork.PhotographerRepository.Edit(Photographer);
            _applicationUnitOfWork.Save();
        }
    }
}
