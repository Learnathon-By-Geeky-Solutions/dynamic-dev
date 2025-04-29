using EasyTravel.Application.Utilities;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class AdminAgencyService : IAdminAgencyService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly ILogger<AdminAgencyService> _logger;

        public AdminAgencyService(IApplicationUnitOfWork applicationUnitOfWork, ILogger<AdminAgencyService> logger)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _logger = logger;
        }
        public void Create(Agency agency)
        {
            ArgumentValidator.ThrowIfNull(agency, nameof(agency));

            try
            {
                _logger.LogInformation("Creating a new agency with Name: {Name}", agency.Name);
                _applicationUnitOfWork.AgencyRepository.Add(agency);
                _applicationUnitOfWork.Save();
                _logger.LogInformation("Agency created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the agency.");
                throw new InvalidOperationException("An error occurred while creating the agency.", ex);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting agency with ID: {Id}", id);

                var agency = _applicationUnitOfWork.AgencyRepository.GetById(id);
                if (agency == null)
                {
                    _logger.LogWarning("Agency with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Agency with ID: {id} not found.");
                }

                _applicationUnitOfWork.AgencyRepository.Remove(id);
                _applicationUnitOfWork.Save();
                _logger.LogInformation("Agency deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the agency with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the agency with ID: {id}.", ex);
            }
        }

        public Agency Get(Guid id)
        {
            return _applicationUnitOfWork.AgencyRepository.GetById(id);
        }

        public IEnumerable<Agency> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all agencies.");
                return _applicationUnitOfWork.AgencyRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all agencies.");
                throw new InvalidOperationException("An error occurred while fetching all agencies.", ex);
            }
        }

        public async Task<PagedResult<Agency>> GetPaginatedAgenciesAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

            try
            {
                _logger.LogInformation("Fetching paginated agencies for page {PageNumber} with size {PageSize}.", pageNumber, pageSize);

                var totalItems = await _applicationUnitOfWork.AgencyRepository.GetCountAsync();
                var agencies = await _applicationUnitOfWork.AgencyRepository.GetAllAsync();

                agencies = agencies.OrderBy(a => a.Name)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

                return new PagedResult<Agency>
                {
                    Items = agencies.ToList(),
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated agencies.");
                throw new InvalidOperationException("An error occurred while fetching paginated agencies.", ex);
            }
        }

        public void Update(Agency agency)
        {
            ArgumentValidator.ThrowIfNull(agency, nameof(agency));

            try
            {
                _logger.LogInformation("Updating agency with ID: {Id}", agency.Id);

                var existingAgency = _applicationUnitOfWork.AgencyRepository.GetById(agency.Id);
                if (existingAgency == null)
                {
                    _logger.LogWarning("Agency with ID: {Id} not found.", agency.Id);
                    throw new KeyNotFoundException($"Agency with ID: {agency.Id} not found.");
                }

                _applicationUnitOfWork.AgencyRepository.Edit(agency);
                _applicationUnitOfWork.Save();
                _logger.LogInformation("Agency updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the agency with ID: {Id}", agency.Id);
                throw new InvalidOperationException($"An error occurred while updating the agency with ID: {agency.Id}.", ex);
            }
        }
    }
}
