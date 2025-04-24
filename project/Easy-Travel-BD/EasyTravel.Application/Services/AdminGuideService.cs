using EasyTravel.Application.Factories;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTravel.Application.Services
{
    public class AdminGuideService : IAdminGuideService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IAdminAgencyService _agencyService;
        private readonly IGuideFactory _guideFactory;
        private readonly ILogger<AdminGuideService> _logger;

        public AdminGuideService(
            IApplicationUnitOfWork unitOfWork,
            IAdminAgencyService agencyService,
            IGuideFactory guideFactory,
            ILogger<AdminGuideService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _agencyService = agencyService ?? throw new ArgumentNullException(nameof(agencyService));
            _guideFactory = guideFactory ?? throw new ArgumentNullException(nameof(guideFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Guide GetGuideInstance()
        {
            try
            {
                _logger.LogInformation("Creating a new guide instance.");
                var agencyList = _agencyService.GetAll();
                var guide = _guideFactory.CreateInstance();
                guide.Agencies = agencyList.ToList();
                _logger.LogInformation("Successfully created a new guide instance.");
                return guide;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new guide instance.");
                throw new InvalidOperationException("An error occurred while creating a new guide instance.", ex);
            }
        }

        public void Create(Guide entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to create a null guide entity.");
                throw new ArgumentNullException(nameof(entity), "Guide entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating a new guide.");
                _unitOfWork.GuideRepository.Add(entity);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully created a new guide.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new guide.");
                throw new InvalidOperationException("An error occurred while creating a new guide.", ex);
            }
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid guide ID provided for deletion.");
                throw new ArgumentException("Guide ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete guide with ID: {Id}", id);
                _unitOfWork.GuideRepository.Remove(id);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted guide with ID: {Id}", id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Guide with ID: {Id} not found for deletion.", id);
                throw new KeyNotFoundException($"Guide with ID: {id} not found.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the guide with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the guide with ID: {id}.", ex);
            }
        }

        public Guide Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid guide ID provided for retrieval.");
                throw new ArgumentException("Guide ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching guide with ID: {Id}", id);
                var guide = _unitOfWork.GuideRepository.GetById(id);
                if (guide == null)
                {
                    _logger.LogWarning("Guide with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Guide with ID: {id} not found.");
                }
                return guide;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the guide with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the guide with ID: {id}.", ex);
            }
        }

        public IEnumerable<Guide> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all guides.");
                return _unitOfWork.GuideRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all guides.");
                throw new InvalidOperationException("An error occurred while fetching all guides.", ex);
            }
        }

        public void Update(Guide entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to update a null guide entity.");
                throw new ArgumentNullException(nameof(entity), "Guide entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating guide with ID: {Id}", entity.Id);
                _unitOfWork.GuideRepository.Edit(entity);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully updated guide with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the guide with ID: {Id}", entity.Id);
                throw new InvalidOperationException($"An error occurred while updating the guide with ID: {entity.Id}.", ex);
            }
        }
    }
}
