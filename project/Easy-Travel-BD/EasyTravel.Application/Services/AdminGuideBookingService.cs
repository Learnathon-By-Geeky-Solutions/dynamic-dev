using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class AdminGuideBookingService : IAdminGuideBookingService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<AdminGuideBookingService> _logger;

        public AdminGuideBookingService(IApplicationUnitOfWork unitOfWork, ILogger<AdminGuideBookingService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid guide booking ID provided for deletion.");
                throw new ArgumentException("Guide booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete guide booking with ID: {Id}", id);
                _unitOfWork.BookingRepository.Remove(id);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted guide booking with ID: {Id}", id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Guide booking with ID: {Id} not found for deletion.", id);
                throw new KeyNotFoundException($"Guide booking with ID: {id} not found.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the guide booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the guide booking with ID: {id}.", ex);
            }
        }

        public GuideBooking Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid guide booking ID provided for retrieval.");
                throw new ArgumentException("Guide booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching guide booking with ID: {Id}", id);
                var guideBooking = _unitOfWork.GuideBookingRepository.GetById(id);
                if (guideBooking == null)
                {
                    _logger.LogWarning("Guide booking with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Guide booking with ID: {id} not found.");
                }
                return guideBooking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the guide booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the guide booking with ID: {id}.", ex);
            }
        }

        public IEnumerable<GuideBooking> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all guide bookings.");
                return _unitOfWork.GuideBookingRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all guide bookings.");
                throw new InvalidOperationException("An error occurred while fetching all guide bookings.", ex);
            }
        }

        public async Task<PagedResult<GuideBooking>> GetPaginatedGuideBookingAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _unitOfWork.GuideBookingRepository.GetCountAsync();

            var bookings = await _unitOfWork.GuideBookingRepository.GetAllAsync();
            bookings = bookings.OrderBy(a => a.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            var result = new PagedResult<GuideBooking>
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
