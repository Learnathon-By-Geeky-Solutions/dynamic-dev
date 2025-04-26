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
    public class GuideBookingService : IGuideBookingService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<GuideBookingService> _logger;

        public GuideBookingService(IApplicationUnitOfWork unitOfWork, ILogger<GuideBookingService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SaveBooking(GuideBooking model, Booking booking, Payment? payment = null)
        {
            if (model == null)
            {
                _logger.LogWarning("Attempted to save a null GuideBooking entity.");
                throw new ArgumentNullException(nameof(model), "GuideBooking entity cannot be null.");
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
                    _logger.LogInformation("Saving booking for guide with ID: {GuideId}", model.GuideId);

                    // Save the booking
                    _unitOfWork.BookingRepository.Add(booking);
                    _unitOfWork.Save();

                    // Save the guide booking
                    _unitOfWork.GuideBookingRepository.Add(model);
                    _unitOfWork.Save();

                    // Save payment if provided
                    if (payment != null)
                    {
                        _unitOfWork.PaymentRepository.Add(payment);
                        _unitOfWork.Save();
                    }

                    // Commit the transaction
                    transaction.Complete();
                    _logger.LogInformation("Successfully saved booking for guide with ID: {GuideId}", model.GuideId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while saving the booking for guide with ID: {GuideId}", model.GuideId);
                    throw new InvalidOperationException($"An error occurred while saving the booking for guide with ID: {model.GuideId}.", ex);
                }
            }
        }

        public bool CancelBooking()
        {
            _logger.LogWarning("CancelBooking method is not implemented.");
            throw new NotImplementedException("CancelBooking method is not implemented.");
        }

        public async Task<IEnumerable<GuideBooking>> GetBookingListByFormDataAsync(GuideBooking model)
        {
            if (model == null)
            {
                _logger.LogWarning("Attempted to fetch booking list with a null GuideBooking model.");
                throw new ArgumentNullException(nameof(model), "GuideBooking model cannot be null.");
            }

            try
            {
                _logger.LogInformation("Fetching booking list for guide with ID: {GuideId}", model.GuideId);

                var difference = DateTime.Now.TimeOfDay - model.StartTime;
                if (difference < TimeSpan.FromHours(3))
                {
                    _logger.LogInformation("No bookings found as the time difference is less than 3 hours.");
                    return new List<GuideBooking>();
                }

                var bookings = await _unitOfWork.GuideBookingRepository.GetAsync(e =>
                    e.EventDate == model.EventDate &&
                    e.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                    (e.Booking!.BookingStatus == BookingStatus.Confirmed || e.Booking.BookingStatus == BookingStatus.Pending) &&
                    (
                        (e.StartTime >= model.StartTime && e.StartTime <= model.EndTime) ||
                        (e.EndTime >= model.StartTime && e.EndTime <= model.EndTime)
                    )
                );

                _logger.LogInformation("Successfully fetched booking list for guide with ID: {GuideId}", model.GuideId);
                return bookings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the booking list for guide with ID: {GuideId}", model.GuideId);
                throw new InvalidOperationException($"An error occurred while fetching the booking list for guide with ID: {model.GuideId}.", ex);
            }
        }

        public async Task<bool> IsBooked(GuideBooking model)
        {
            if (model == null)
            {
                _logger.LogWarning("Attempted to check booking status with a null GuideBooking model.");
                throw new ArgumentNullException(nameof(model), "GuideBooking model cannot be null.");
            }

            try
            {
                _logger.LogInformation("Checking if guide with ID: {GuideId} is booked.", model.GuideId);

                var bookings = await GetBookingListByFormDataAsync(model);
                var isBooked = bookings.Any(e => e.GuideId == model.GuideId);

                _logger.LogInformation("Guide with ID: {GuideId} is {Status}.", model.GuideId, isBooked ? "booked" : "not booked");
                return isBooked;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if guide with ID: {GuideId} is booked.", model.GuideId);
                throw new InvalidOperationException($"An error occurred while checking if guide with ID: {model.GuideId} is booked.", ex);
            }
        }
    }
}



