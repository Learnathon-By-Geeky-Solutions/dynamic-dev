using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class AdminHotelBookingService : IAdminHotelBookingService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<AdminHotelBookingService> _logger;

        public AdminHotelBookingService(IApplicationUnitOfWork unitOfWork, ILogger<AdminHotelBookingService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public HotelBooking Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid hotel booking ID provided for retrieval.");
                throw new ArgumentException("Hotel booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching hotel booking with ID: {Id}", id);
                var hotelBooking = _unitOfWork.HotelBookingRepository.GetById(id);
                if (hotelBooking == null)
                {
                    _logger.LogWarning("Hotel booking with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Hotel booking with ID: {id} not found.");
                }
                return hotelBooking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the hotel booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the hotel booking with ID: {id}.", ex);
            }
        }

        public IEnumerable<HotelBooking> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all hotel bookings.");
                return _unitOfWork.HotelBookingRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all hotel bookings.");
                throw new InvalidOperationException("An error occurred while fetching all hotel bookings.", ex);
            }
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid hotel booking ID provided for deletion.");
                throw new ArgumentException("Hotel booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete hotel booking with ID: {Id}", id);
                _unitOfWork.BookingRepository.Remove(id);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted hotel booking with ID: {Id}", id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex,"Hotel booking with ID: {Id} not found for deletion.", id);
                throw new KeyNotFoundException($"Hotel booking with ID: {id} not found.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the hotel booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the hotel booking with ID: {id}.", ex);
            }
        } 
    }
}
