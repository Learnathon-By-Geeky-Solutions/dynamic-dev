using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Enums;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class GuideService : IGuideService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<GuideService> _logger;

        public GuideService(IApplicationUnitOfWork unitOfWork, ILogger<GuideService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Guide Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid guide ID provided for retrieval.");
                throw new ArgumentException("Guide ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching guide with ID: {Id}", id);
                var guide = _unitOfWork.GuideRepository.GetById(id);
                return guide;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the guide with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the guide with ID: {id}.", ex);
            }
        }

        public IEnumerable<Guide> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all guides.");
                return _unitOfWork.GuideRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all guides.");
                throw new InvalidOperationException("An error occurred while fetching all guides.", ex);
            }
        }

        public async Task<(IEnumerable<Guide>,int)> GetGuideListAsync(GuideBooking guideBooking, int pageNumber, int pageSize)
        {
            if (guideBooking == null)
            {
                _logger.LogWarning("Attempted to fetch guide list with a null GuideBooking model.");
                throw new ArgumentNullException(nameof(guideBooking), "GuideBooking model cannot be null.");
            }
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");
            try
            {
                _logger.LogInformation("Fetching guide list for event on {EventDate} at {StartTime} for page {PageNumber} with size {PageSize}", guideBooking.EventDate, guideBooking.StartTime,pageNumber,pageSize);

                DateTime now = DateTime.Now;
                DateTime selectedDateTime = guideBooking.EventDate.Add(guideBooking.StartTime);
                TimeSpan difference = selectedDateTime - now;

                if (selectedDateTime < now || difference < TimeSpan.FromHours(6))
                {
                    _logger.LogInformation("No guides available as the selected time is less than 6 hours from now.");
                    return (new List<Guide>(),0);
                }
                var guides = await _unitOfWork.GuideRepository.GetAsync(
                    e => e.Availability &&
                        (!e.GuideBookings!.Any() ||
                         e.GuideBookings!.Any(
                             p => p.Booking!.BookingStatus != BookingStatus.Confirmed &&
                                  p.Booking.BookingStatus != BookingStatus.Pending &&
                                  p.StartTime > DateTime.Now.AddHours(6).TimeOfDay &&
                                  p.EventDate >= guideBooking.EventDate &&
                                  (
                                      (p.StartTime >= guideBooking.StartTime && p.EventDate >= guideBooking.EventDate) ||
                                      (p.EndTime >= guideBooking.StartTime && p.EventDate >= guideBooking.EventDate)
                                  )
                         ))
                );
                int totalItems = guides.Count;
                var paginatedGuides = guides
                    .OrderBy(g => g.FirstName) // Sort by first name
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                _logger.LogInformation("Successfully fetched guide list for event on {EventDate} at {StartTime}", guideBooking.EventDate, guideBooking.StartTime);
                return (paginatedGuides, (int)Math.Ceiling(totalItems / (double)pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the guide list for event on {EventDate} at {StartTime}", guideBooking.EventDate, guideBooking.StartTime);
                throw new InvalidOperationException($"An error occurred while fetching the guide list for event on {guideBooking.EventDate} at {guideBooking.StartTime}.", ex);
            }
        }

    }
}




