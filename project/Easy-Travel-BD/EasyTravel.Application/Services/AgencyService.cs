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
    public class AgencyService : IAgencyService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public AgencyService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _unitOfWork = applicationUnitOfWork;
        }
        public Agency Get(Guid id)
        {
            return _unitOfWork.AgencyRepository.GetById(id);
        }

        public IEnumerable<Agency> GetAll()
        {
            return _unitOfWork.AgencyRepository.GetAll();
        }
    }
}
