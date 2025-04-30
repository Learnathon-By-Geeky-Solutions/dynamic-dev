using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IApplicationUnitOfWork unitOfWork, ILogger<BookingService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Booking Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid booking ID provided for retrieval.");
                throw new ArgumentException("Booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching booking with ID: {Id}", id);
                var booking = _unitOfWork.BookingRepository.GetById(id);
                return booking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the booking with ID: {id}.", ex);
            }
        }

        public IEnumerable<Booking> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all bookings.");
                return _unitOfWork.BookingRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all bookings.");
                throw new InvalidOperationException("An error occurred while fetching all bookings.", ex);
            }
        }

        public void RemoveBooking(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid booking ID provided for cancellation.");
                throw new ArgumentException("Booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to cancel booking with ID: {Id}", id);

                // Fetch the booking
                var booking = _unitOfWork.BookingRepository.GetById(id);
                if (booking == null)
                {
                    _logger.LogWarning("Booking with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Booking with ID: {id} not found.");
                }

                // Save changes to the database
                _unitOfWork.BookingRepository.RemoveAsync(id);
                _unitOfWork.Save();

                _logger.LogInformation("Booking with ID: {Id} successfully cancelled.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cancelling the booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while cancelling the booking with ID: {id}.", ex);
            }
        }

        public void AddBooking(Booking booking)
        {
            if (booking == null)
            {
                _logger.LogWarning("Null booking object provided for addition.");
                throw new ArgumentNullException(nameof(booking), "Booking cannot be null.");
            }

            try
            {
                _logger.LogInformation("Adding a new booking with ID: {Id}", booking.Id);

                // Add the booking to the repository
                _unitOfWork.BookingRepository.Add(booking);
                _unitOfWork.Save();

                _logger.LogInformation("Booking with ID: {Id} successfully added.", booking.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the booking with ID: {Id}", booking.Id);
                throw new InvalidOperationException($"An error occurred while adding the booking with ID: {booking.Id}.", ex);
            }
        }
    }
}


