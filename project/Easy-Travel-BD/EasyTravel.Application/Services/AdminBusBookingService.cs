using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class AdminBusBookingService : IAdminBusBookingService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<AdminBusBookingService> _logger;

        public AdminBusBookingService(IApplicationUnitOfWork unitOfWork, ILogger<AdminBusBookingService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void DeleteBusBooking(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                _logger.LogWarning("Invalid bus booking ID provided for deletion.");
                throw new ArgumentException("Bus booking ID cannot be empty.", nameof(Id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete bus booking with Id: {Id}", Id);
                _unitOfWork.BusBookingRepository.DeleteBusBooking(Id);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted bus booking with Id: {Id}", Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the bus booking with Id: {Id}", Id);
                throw new InvalidOperationException($"An error occurred while deleting the bus booking with Id: {Id}.", ex);
            }
        }

        public IEnumerable<BusBooking> GetAllBusBookings()
        {
            try
            {
                _logger.LogInformation("Fetching all bus bookings.");
                return _unitOfWork.BusBookingRepository.GetAllBusBookings();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all bus bookings.");
                throw new InvalidOperationException("An error occurred while fetching all bus bookings.", ex);
            }
        }

        public async Task<PagedResult<BusBooking>> GetPaginatedBusBookingsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _unitOfWork.BusBookingRepository.GetCountAsync();

            var bookings = await _unitOfWork.BusBookingRepository.GetAllAsync();
                bookings = bookings.OrderBy(a => a.PassengerName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<BusBooking>
            {
                Items = bookings.ToList(),
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }
    }
}
