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
    public class AgencyService : IAgencyService
    {
        private readonly IApplicationUnitOfWork _applicationunitOfWork;
        public AgencyService(IApplicationUnitOfWork applicationunitOfWork)
        {
            _applicationunitOfWork = applicationunitOfWork;
        }
        public void AddAgency(Agency agency)
        {
            _applicationunitOfWork.AgencyRepository.Add(agency);
            _applicationunitOfWork.Save();
        }

        public void DeleteAgency(Agency agency)
        {
            _applicationunitOfWork.AgencyRepository.Remove(agency);
            _applicationunitOfWork.Save();
        }

        public void UpdateAgency(Agency agency)
        {
            _applicationunitOfWork.AgencyRepository.Edit(agency);
            _applicationunitOfWork.Save();
        }
    }
}
