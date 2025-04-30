using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace EasyTravel.Application.Services
{
    public class HotelBookingService : IHotelBookingService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<HotelBookingService> _logger;
        private readonly IBookingService _bookingService;
        public HotelBookingService(IApplicationUnitOfWork unitOfWork, ILogger<HotelBookingService> logger,IBookingService bookingService)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bookingService = bookingService;
        }
        public void SaveBooking(HotelBooking model, Booking booking, Payment? payment = null)
        {
            if (model == null)
            {
                _logger.LogWarning("Attempted to save a null HotelBooking entity.");
                throw new ArgumentNullException(nameof(model), "HotelBooking entity cannot be null.");
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
                    _logger.LogInformation("Saving booking for guide with ID: {HotelId}", model.HotelId);
                    if (_bookingService.Get(booking.Id) != null)
                    {                     // Save the booking
                        _unitOfWork.BookingRepository.Edit(booking);
                        _unitOfWork.Save();
                    }
                    model.Id = booking.Id; // Set the booking ID to the photographer booking model
                                           // Save the photographer booking
                    _unitOfWork.HotelBookingRepository.Add(model);
                    _unitOfWork.Save();

                    // Save payment if provided
                    if (payment != null)
                    {
                        _unitOfWork.PaymentRepository.Add(payment);
                        _unitOfWork.Save();
                    }

                    // Commit the transaction
                    transaction.Complete();
                    _logger.LogInformation("Saving booking for guide with ID: {HotelId}", model.HotelId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while saving the booking for guide with ID: {GuideId}", model.HotelId);
                    throw new InvalidOperationException($"An error occurred while saving the booking for guide with ID: {model.HotelId}.", ex);
                }
            }
        }

        public void Create(HotelBooking entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to create a null HotelBooking entity.");
                throw new ArgumentNullException(nameof(entity), "HotelBooking entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating a new hotel booking with ID: {Id}", entity.Id);
                _unitOfWork.HotelBookingRepository.Add(entity);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully created hotel booking with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the hotel booking with ID: {Id}", entity.Id);
                throw new InvalidOperationException($"An error occurred while creating the hotel booking with ID: {entity.Id}.", ex);
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
                _logger.LogInformation("Deleting hotel booking with ID: {Id}", id);
                var booking = _unitOfWork.HotelBookingRepository.GetById(id);
                if (booking == null)
                {
                    _logger.LogWarning("Hotel booking with ID: {Id} not found for deletion.", id);
                    throw new KeyNotFoundException($"Hotel booking with ID: {id} not found.");
                }

                _unitOfWork.HotelBookingRepository.Remove(booking);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted hotel booking with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the hotel booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the hotel booking with ID: {id}.", ex);
            }
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
                var booking = _unitOfWork.HotelBookingRepository.GetById(id);
                return booking;
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

        public void Update(HotelBooking entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to update a null HotelBooking entity.");
                throw new ArgumentNullException(nameof(entity), "HotelBooking entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating hotel booking with ID: {Id}", entity.Id);
                _unitOfWork.HotelBookingRepository.Edit(entity);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully updated hotel booking with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the hotel booking with ID: {Id}", entity.Id);
                throw new InvalidOperationException($"An error occurred while updating the hotel booking with ID: {entity.Id}.", ex);
            }
        }
    }
}




