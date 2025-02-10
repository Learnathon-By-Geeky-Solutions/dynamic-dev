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

        public Photographer GetInstance()
        {
            var model = new Photographer()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                ContactNumber = string.Empty,
                Address = string.Empty,
                ProfilePicture = string.Empty,
                Bio = string.Empty,
                DateOfBirth = DateTime.MinValue,
                Skills = string.Empty,
                PortfolioUrl = string.Empty,
                Specialization = string.Empty,
                YearsOfExperience = 0,
                Availability = false,
                HourlyRate = 0,
                Rating = 0,
                SocialMediaLinks = null,
                Status = null,
                AgencyId = default
            };
            return model;
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
