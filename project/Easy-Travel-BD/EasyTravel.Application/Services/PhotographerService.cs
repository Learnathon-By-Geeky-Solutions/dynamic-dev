using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Enums;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
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
            _unitOfWork = unitOfWork ;
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

        public async Task<(IEnumerable<Photographer>,int)> GetPhotographerListAsync(PhotographerBooking photographerBooking,int pageNumber,int pageSize)
        {
            if (photographerBooking == null)
            {
                _logger.LogWarning("Attempted to fetch photographer list with a null PhotographerBooking model.");
                throw new ArgumentNullException(nameof(photographerBooking), "PhotographerBooking model cannot be null.");
            }
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

            try
            {
                _logger.LogInformation("Fetching photographers list for event on {EventDate} at {StartTime} for page {PageNumber} with size {PageSize}", photographerBooking.EventDate, photographerBooking.StartTime, pageNumber, pageSize);
                DateTime now = DateTime.Now;
                DateTime selectedDateTime = photographerBooking.EventDate.Add(photographerBooking.StartTime);
                TimeSpan difference = selectedDateTime - now;

                if (selectedDateTime < now || difference < TimeSpan.FromHours(6))
                {
                    _logger.LogInformation("No guides available as the selected time is less than 6 hours from now.");
                    return (new List<Photographer>(),0);
                }
              
                var photographers = await _unitOfWork.PhotographerRepository.GetAsync(
                    e => e.Availability &&
                        (!e.PhotographerBookings!.Any() ||
                         e.PhotographerBookings!.Any(
                             p => p.Booking!.BookingStatus != BookingStatus.Confirmed &&
                                  p.Booking.BookingStatus != BookingStatus.Pending &&
                                  p.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                                  p.EventDate >= photographerBooking.EventDate &&
                                  (
                                      (p.StartTime >= photographerBooking.StartTime && p.EventDate >= photographerBooking.EventDate) ||
                                      (p.EndTime >= photographerBooking.StartTime && p.EventDate >= photographerBooking.EventDate)
                                  )
                         ))
                );
               
                var totalItems = photographers.Count();
                var paginatedPhotographers = photographers.
                    OrderBy(p => p.FirstName).
                    Skip((pageNumber - 1) * pageSize).
                    Take(pageSize).
                    ToList();

                _logger.LogInformation("Successfully fetched photographer list for event on {EventDate} at {StartTime}", photographerBooking.EventDate, photographerBooking.StartTime);
                return (paginatedPhotographers, (int)Math.Ceiling(totalItems / (double)pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the photographer list for event on {EventDate} at {StartTime}", photographerBooking.EventDate, photographerBooking.StartTime);
                throw new InvalidOperationException($"An error occurred while fetching the photographer list for event on {photographerBooking.EventDate} at {photographerBooking.StartTime}.", ex);
            }
        }
    }
}
