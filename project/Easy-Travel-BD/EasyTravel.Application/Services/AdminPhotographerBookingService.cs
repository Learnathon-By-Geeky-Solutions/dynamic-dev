using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class AdminPhotographerBookingService : IAdminPhotographerBookingService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<AdminPhotographerBookingService> _logger;

        public AdminPhotographerBookingService(IApplicationUnitOfWork unitOfWork, ILogger<AdminPhotographerBookingService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid photographer booking ID provided for deletion.");
                throw new ArgumentException("Photographer booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete photographer booking with ID: {Id}", id);
                _unitOfWork.BookingRepository.Remove(id);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted photographer booking with ID: {Id}", id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex,"Photographer booking with ID: {Id} not found for deletion.", id);
                throw new KeyNotFoundException($"Photographer booking with ID: {id} not found.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the photographer booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the photographer booking with ID: {id}.", ex);
            }
        }

        public PhotographerBooking Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid photographer booking ID provided for retrieval.");
                throw new ArgumentException("Photographer booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching photographer booking with ID: {Id}", id);
                var photographerBooking = _unitOfWork.PhotographerBookingRepository.GetById(id);
                if (photographerBooking == null)
                {
                    _logger.LogWarning("Photographer booking with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Photographer booking with ID: {id} not found.");
                }
                return photographerBooking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the photographer booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the photographer booking with ID: {id}.", ex);
            }
        }

        public IEnumerable<PhotographerBooking> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all photographer bookings.");
                return _unitOfWork.PhotographerBookingRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all photographer bookings.");
                throw new InvalidOperationException("An error occurred while fetching all photographer bookings.", ex);
            }
        }

        public async Task<PagedResult<PhotographerBooking>> GetPaginatedPhotographerBookingAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _unitOfWork.PhotographerBookingRepository.GetCountAsync();

            var bookings = await _unitOfWork.PhotographerBookingRepository.GetAllAsync();
            bookings = bookings.OrderBy(a => a.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            var result = new PagedResult<PhotographerBooking>
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

