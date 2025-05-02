using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
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

        public async Task<PagedResult<BusBooking>> GetBusBookingsAsync(Guid id, int pageNumber, int pageSize)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching bus bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching bus bookings for user with ID: {Id}", id);
                var bookings = await _unitOfWork.BusBookingRepository.GetAsync(e =>e.Booking != null && e.Booking.UserId == id);
                var totalItems = bookings.Count();
                var pagedBookings = bookings.OrderBy(e => e.BookingDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var pagedResult = new PagedResult<BusBooking>
                {
                    Items = pagedBookings,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                return pagedResult;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching bus bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching bus bookings for user with ID: {id}.", ex);
            }
        }

        public async Task<PagedResult<CarBooking>> GetCarBookingsAsync(Guid id, int pageNumber, int pageSize)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching car bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching car bookings for user with ID: {Id}", id);
                var bookings = await _unitOfWork.CarBookingRepository.GetAsync(e => e.Booking != null && e.Booking.UserId == id);
                var totalItems = bookings.Count();
                var pagedBookings = bookings.OrderBy(e => e.BookingDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var pagedResult = new PagedResult<CarBooking>
                {
                    Items = pagedBookings,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                return pagedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching car bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching car bookings for user with ID: {id}.", ex);
            }
        }

        public async Task<PagedResult<GuideBooking>> GetGuideBookingsAsync(Guid id, int pageNumber, int pageSize)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching guide bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching guide bookings for user with ID: {Id}", id);
                var bookings = await _unitOfWork.GuideBookingRepository.GetAsync(e => e.Booking != null && e.Booking.UserId == id);
                var totalItems = bookings.Count();
                var pagedBookings = bookings.OrderBy(e => e.Email).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var pagedResult = new PagedResult<GuideBooking>
                {
                    Items = pagedBookings,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                return pagedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching guide bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching guide bookings for user with ID: {id}.", ex);
            }
        }

        public async Task<PagedResult<HotelBooking>> GetHotelBookingsAsync(Guid id, int pageNumber, int pageSize)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching hotel bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching hotel bookings for user with ID: {Id}", id);
                var bookings = await _unitOfWork.HotelBookingRepository.GetAsync(e => e.Booking != null && e.Booking.UserId == id);
                var totalItems = bookings.Count();
                var pagedBookings = bookings.OrderBy(e => e.CheckInDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var pagedResult = new PagedResult<HotelBooking>
                {
                    Items = pagedBookings,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                return pagedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching hotel bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching hotel bookings for user with ID: {id}.", ex);
            }
        }

        public async Task<PagedResult<PhotographerBooking>> GetPhotographerBookingsAsync(Guid id, int pageNumber, int pageSize)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID provided for fetching photographer bookings.");
                throw new ArgumentException(UserIdCannotBeEmptyMessage, nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching photographer bookings for user with ID: {Id}", id);
                var bookings = await _unitOfWork.PhotographerBookingRepository.GetAsync(e => e.Booking != null && e.Booking.UserId == id);
                var totalItems = bookings.Count();
                var pagedBookings = bookings.OrderBy(e => e.EventDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var pagedResult = new PagedResult<PhotographerBooking>
                {
                    Items = pagedBookings,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                return pagedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching photographer bookings for user with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching photographer bookings for user with ID: {id}.", ex);
            }
        }
    }
}


