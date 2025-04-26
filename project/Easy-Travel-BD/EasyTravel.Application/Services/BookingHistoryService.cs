using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class BookingHistoryService : IBookingHistoryService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<BookingHistoryService> _logger;
        private const string UserIdCannotBeEmptyMessage = "User ID cannot be empty.";
        public BookingHistoryService(IApplicationUnitOfWork unitOfWork, ILogger<BookingHistoryService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BusBooking>> GetBusBookingsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching bus bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching bus bookings for user with ID: {Id}", id);
                return await _unitOfWork.BusBookingRepository.GetAsync(e =>e.Booking != null && e.Booking.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching bus bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching bus bookings for user with ID: {id}.", ex);
            }
        }

        public async Task<IEnumerable<CarBooking>> GetCarBookingsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching car bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching car bookings for user with ID: {Id}", id);
                return await _unitOfWork.CarBookingRepository.GetAsync(e => e.Booking != null && e.Booking.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching car bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching car bookings for user with ID: {id}.", ex);
            }
        }

        public async Task<IEnumerable<GuideBooking>> GetGuideBookingsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching guide bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching guide bookings for user with ID: {Id}", id);
                return await _unitOfWork.GuideBookingRepository.GetAsync(e => e.Booking != null && e.Booking.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching guide bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching guide bookings for user with ID: {id}.", ex);
            }
        }

        public async Task<IEnumerable<HotelBooking>> GetHotelBookingsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching hotel bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching hotel bookings for user with ID: {Id}", id);
                return await _unitOfWork.HotelBookingRepository.GetAsync(e => e.Booking != null && e.Booking.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching hotel bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching hotel bookings for user with ID: {id}.", ex);
            }
        }

        public async Task<IEnumerable<PhotographerBooking>> GetPhotographerBookingsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching photographer bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching photographer bookings for user with ID: {Id}", id);
                return await _unitOfWork.PhotographerBookingRepository.GetAsync(e => e.Booking != null && e.Booking.UserId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching photographer bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching photographer bookings for user with ID: {id}.", ex);
            }
        }
    }
}


