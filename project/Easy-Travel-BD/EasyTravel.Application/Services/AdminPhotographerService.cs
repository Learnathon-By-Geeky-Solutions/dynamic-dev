using EasyTravel.Application.Factories;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Factories;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTravel.Application.Services
{
    public class AdminPhotographerService : IAdminPhotographerService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly IAdminAgencyService _agencyService;
        private readonly IPhotographerFactory _photographerFactory;
        private readonly ILogger<AdminPhotographerService> _logger;

        public AdminPhotographerService(
            IApplicationUnitOfWork unitOfWork,
            IAdminAgencyService agencyService,
            IPhotographerFactory photographerFactory,
            ILogger<AdminPhotographerService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _agencyService = agencyService ?? throw new ArgumentNullException(nameof(agencyService));
            _photographerFactory = photographerFactory ?? throw new ArgumentNullException(nameof(photographerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Photographer GetPhotographerInstance()
        {
            try
            {
                _logger.LogInformation("Creating a new photographer instance.");
                var agencyList = _agencyService.GetAll();
                var photographer = _photographerFactory.CreateInstance();
                photographer.Agencies = agencyList.ToList();
                _logger.LogInformation("Successfully created a new photographer instance.");
                return photographer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new photographer instance.");
                throw new InvalidOperationException("An error occurred while creating a new photographer instance.", ex);
            }
        }

        public void Create(Photographer photographer)
        {
            if (photographer == null)
            {
                _logger.LogWarning("Attempted to create a null photographer entity.");
                throw new ArgumentNullException(nameof(photographer), "Photographer entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating a new photographer.");
                _unitOfWork.PhotographerRepository.Add(photographer);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully created a new photographer.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new photographer.");
                throw new InvalidOperationException("An error occurred while creating a new photographer.", ex);
            }
        }

        public void Update(Photographer photographer)
        {
            if (photographer == null)
            {
                _logger.LogWarning("Attempted to update a null photographer entity.");
                throw new ArgumentNullException(nameof(photographer), "Photographer entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating photographer with ID: {Id}", photographer.Id);
                _unitOfWork.PhotographerRepository.Edit(photographer);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully updated photographer with ID: {Id}", photographer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the photographer with ID: {Id}", photographer.Id);
                throw new InvalidOperationException($"An error occurred while updating the photographer with ID: {photographer.Id}.", ex);
            }
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid photographer ID provided for deletion.");
                throw new ArgumentException("Photographer ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete photographer with ID: {Id}", id);
                _unitOfWork.PhotographerRepository.Remove(id);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted photographer with ID: {Id}", id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex,"Photographer with ID: {Id} not found for deletion.", id);
                throw new KeyNotFoundException($"Photographer with ID: {id} not found.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the photographer with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the photographer with ID: {id}.", ex);
            }
        }

        public Photographer Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid photographer ID provided for retrieval.");
                throw new ArgumentException("Photographer ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching photographer with ID: {Id}", id);
                var agencyList = _agencyService.GetAll();
                var photographer = _unitOfWork.PhotographerRepository.GetById(id);
                if (photographer == null)
                {
                    _logger.LogWarning("Photographer with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Photographer with ID: {id} not found.");
                }
                photographer.Agencies = agencyList.ToList();
                return photographer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the photographer with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the photographer with ID: {id}.", ex);
            }
        }

        public IEnumerable<Photographer> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all photographers.");
                return _unitOfWork.PhotographerRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all photographers.");
                throw new InvalidOperationException("An error occurred while fetching all photographers.", ex);
            }
        }

        public async Task<PagedResult<Photographer>> GetPaginatedPhotographersAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _unitOfWork.PhotographerRepository.GetCountAsync();

            var bookings = await _unitOfWork.PhotographerRepository.GetAllAsync();
            bookings = bookings.OrderBy(a => a.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            var result = new PagedResult<Photographer>
            {
                Items = bookings.ToList(),
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }
    }
}

