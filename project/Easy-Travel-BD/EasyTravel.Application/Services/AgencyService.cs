using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class AgencyService : IAgencyService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<AgencyService> _logger;

        public AgencyService(IApplicationUnitOfWork applicationUnitOfWork, ILogger<AgencyService> logger)
        {
            _unitOfWork = applicationUnitOfWork ?? throw new ArgumentNullException(nameof(applicationUnitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Agency Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid agency ID provided for retrieval.");
                throw new ArgumentException("Agency ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching agency with ID: {Id}", id);
                var agency = _unitOfWork.AgencyRepository.GetById(id);
                return agency;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the agency with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the agency with ID: {id}.", ex);
            }
        }

        public IEnumerable<Agency> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all agencies.");
                return _unitOfWork.AgencyRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all agencies.");
                throw new InvalidOperationException("An error occurred while fetching all agencies.", ex);
            }
        }
    }
}

