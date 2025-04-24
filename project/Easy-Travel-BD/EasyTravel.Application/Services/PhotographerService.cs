using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class PhotographerService : IPhotographerService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<PhotographerService> _logger;

        public PhotographerService(IApplicationUnitOfWork unitOfWork, ILogger<PhotographerService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Photographer Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid photographer ID provided for retrieval.");
                throw new ArgumentException("Photographer ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching photographer with ID: {Id}", id);
                var photographer = _unitOfWork.PhotographerRepository.GetById(id);
                if (photographer == null)
                {
                    _logger.LogWarning("Photographer with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Photographer with ID: {id} not found.");
                }
                return photographer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the photographer with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the photographer with ID: {id}.", ex);
            }
        }

        public IEnumerable<Photographer> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all photographers.");
                return _unitOfWork.PhotographerRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all photographers.");
                throw new InvalidOperationException("An error occurred while fetching all photographers.", ex);
            }
        }

        public async Task<IEnumerable<Photographer>> GetPhotographerListAsync(PhotographerBooking photographerBooking)
        {
            if (photographerBooking == null)
            {
                _logger.LogWarning("Attempted to fetch photographer list with a null PhotographerBooking model.");
                throw new ArgumentNullException(nameof(photographerBooking), "PhotographerBooking model cannot be null.");
            }

            try
            {
                _logger.LogInformation("Fetching photographer list for event on {EventDate} at {StartTime}", photographerBooking.EventDate, photographerBooking.StartTime);

                DateTime now = DateTime.Now;
                DateTime selectedDateTime = photographerBooking.EventDate.Add(photographerBooking.StartTime);
                TimeSpan difference = selectedDateTime - now;

                if (selectedDateTime < now || difference < TimeSpan.FromHours(6))
                {
                    _logger.LogInformation("No photographers available as the selected time is less than 6 hours from now.");
                    return new List<Photographer>();
                }

                var photographers = await _unitOfWork.PhotographerRepository.GetAsync(
                    e => e.Availability &&
                        (!e.PhotographerBookings!.Any() ||
                         e.PhotographerBookings!.Any(
                             p => p.Booking!.BookingStatus != BookingStatus.Confirmed &&
                                  p.Booking.BookingStatus != BookingStatus.Pending &&
                                  p.EventDate >= photographerBooking.EventDate &&
                                  p.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                                  (
                                      (p.StartTime >= photographerBooking.StartTime && p.EventDate >= photographerBooking.EventDate) ||
                                      (p.EndTime >= photographerBooking.StartTime && p.EventDate >= photographerBooking.EventDate)
                                  )
                         ))
                );

                _logger.LogInformation("Successfully fetched photographer list for event on {EventDate} at {StartTime}", photographerBooking.EventDate, photographerBooking.StartTime);
                return photographers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the photographer list for event on {EventDate} at {StartTime}", photographerBooking.EventDate, photographerBooking.StartTime);
                throw new InvalidOperationException($"An error occurred while fetching the photographer list for event on {photographerBooking.EventDate} at {photographerBooking.StartTime}.", ex);
            }
        }
    }
}
