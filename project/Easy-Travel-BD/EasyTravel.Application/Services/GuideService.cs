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
        private IApplicationUnitOfWork _applicationUnitOfWork;
        public GuideService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
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

        public Guide GetInstance()
        {
            var model = new Guide
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                ContactNumber = string.Empty,
                Address = string.Empty,
                ProfilePicture = string.Empty,
                Bio = string.Empty,
                LanguagesSpoken = string.Empty,
                LicenseNumber = string.Empty,
                DateOfBirth = DateTime.MinValue,
                Specialization = string.Empty,
                YearsOfExperience = 0,
                Availability = false,
                HourlyRate = 0,
                Rating = 0,
                Status = null,
                AgencyId = default
            };
            return model;
        }

        public void Update(Guide entity)
        {
            _applicationUnitOfWork.GuideRepository.Edit(entity);
            _applicationUnitOfWork.Save();
        }
    }
}
