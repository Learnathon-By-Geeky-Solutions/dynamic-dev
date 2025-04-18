﻿using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class AdminAgencyService : IAdminAgencyService
    {
        private readonly IApplicationUnitOfWork _applicationunitOfWork;
        public AdminAgencyService(IApplicationUnitOfWork applicationunitOfWork)
        {
            _applicationunitOfWork = applicationunitOfWork;
        }

        public void Create(Agency agency)
        {
            _applicationunitOfWork.AgencyRepository.Add(agency);
            _applicationunitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            _applicationunitOfWork.AgencyRepository.Remove(id);
            _applicationunitOfWork.Save();
        }

        public Agency Get(Guid id)
        {
            return _applicationunitOfWork.AgencyRepository.GetById(id);
        }

        public IEnumerable<Agency> GetAll()
        {
            return _applicationunitOfWork.AgencyRepository.GetAll();
        }
        public void Update(Agency agency)
        {
            _applicationunitOfWork.AgencyRepository.Edit(agency);
            _applicationunitOfWork.Save();
        }
    }
}
