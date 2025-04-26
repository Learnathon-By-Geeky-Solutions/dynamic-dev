using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EasyTravel.Application.Services
{
    public class PhotographerBookingService : IPhotographerBookingService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<PhotographerBookingService> _logger;

        public PhotographerBookingService(IApplicationUnitOfWork unitOfWork, ILogger<PhotographerBookingService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SaveBooking(PhotographerBooking model, Booking booking, Payment? payment = null)
        {
            if (model == null)
            {
                _logger.LogWarning("Attempted to save a null PhotographerBooking entity.");
                throw new ArgumentNullException(nameof(model), "PhotographerBooking entity cannot be null.");
            }

            if (booking == null)
            {
                _logger.LogWarning("Attempted to save a null Booking entity.");
                throw new ArgumentNullException(nameof(booking), "Booking entity cannot be null.");
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    _logger.LogInformation("Saving booking for photographer with ID: {PhotographerId}", model.PhotographerId);

                    // Save the booking
                    _unitOfWork.BookingRepository.Add(booking);
                    _unitOfWork.Save();

                    // Save the photographer booking
                    _unitOfWork.PhotographerBookingRepository.Add(model);
                    _unitOfWork.Save();

                    // Save payment if provided
                    if (payment != null)
                    {
                        _unitOfWork.PaymentRepository.Add(payment);
                        _unitOfWork.Save();
                    }

                    // Commit the transaction
                    transaction.Complete();
                    _logger.LogInformation("Successfully saved booking for photographer with ID: {PhotographerId}", model.PhotographerId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while saving the booking for photographer with ID: {PhotographerId}", model.PhotographerId);
                    throw new InvalidOperationException($"An error occurred while saving the booking for photographer with ID: {model.PhotographerId}.", ex);
                }
            }
        }

        public bool CancelBooking()
        {
            _logger.LogWarning("CancelBooking method is not implemented.");
            throw new NotImplementedException("CancelBooking method is not implemented.");
        }

        public async Task<IEnumerable<PhotographerBooking>> GetBookingListByFormDataAsync(PhotographerBooking model)
        {
            if (model == null)
            {
                _logger.LogWarning("Attempted to fetch booking list with a null PhotographerBooking model.");
                throw new ArgumentNullException(nameof(model), "PhotographerBooking model cannot be null.");
            }

            try
            {
                _logger.LogInformation("Fetching booking list for photographer with ID: {PhotographerId}", model.PhotographerId);

                var difference = DateTime.Now.TimeOfDay - model.StartTime;
                if (difference < TimeSpan.FromHours(6))
                {
                    _logger.LogInformation("No bookings found as the time difference is less than 6 hours.");
                    return new List<PhotographerBooking>();
                }

                var bookings = await _unitOfWork.PhotographerBookingRepository.GetAsync(e =>
                    e.EventDate >= model.EventDate &&
                    e.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                    (e.Booking!.BookingStatus == BookingStatus.Confirmed || e.Booking.BookingStatus == BookingStatus.Pending) &&
                    (
                        (e.StartTime >= model.StartTime && e.StartTime <= model.EndTime) ||
                        (e.EndTime >= model.StartTime && e.EndTime <= model.EndTime)
                    )
                );

                _logger.LogInformation("Successfully fetched booking list for photographer with ID: {PhotographerId}", model.PhotographerId);
                return bookings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the booking list for photographer with ID: {PhotographerId}", model.PhotographerId);
                throw new InvalidOperationException($"An error occurred while fetching the booking list for photographer with ID: {model.PhotographerId}.", ex);
            }
        }

        public async Task<bool> IsBooked(PhotographerBooking model)
        {
            if (model == null)
            {
                _logger.LogWarning("Attempted to check booking status with a null PhotographerBooking model.");
                throw new ArgumentNullException(nameof(model), "PhotographerBooking model cannot be null.");
            }

            try
            {
                _logger.LogInformation("Checking if photographer with ID: {PhotographerId} is booked.", model.PhotographerId);

                var bookings = await GetBookingListByFormDataAsync(model);
                var isBooked = bookings.Any(e => e.PhotographerId == model.PhotographerId);

                _logger.LogInformation("Photographer with ID: {PhotographerId} is {Status}.", model.PhotographerId, isBooked ? "booked" : "not booked");
                return isBooked;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if photographer with ID: {PhotographerId} is booked.", model.PhotographerId);
                throw new InvalidOperationException($"An error occurred while checking if photographer with ID: {model.PhotographerId} is booked.", ex);
            }
        }
    }
}