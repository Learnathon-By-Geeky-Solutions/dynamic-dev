using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class BookingService : IGetService<Booking, Guid>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IApplicationUnitOfWork unitOfWork, ILogger<BookingService> logger)
        {
            _unitOfWork = unitOfWork ;
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
    }
}


