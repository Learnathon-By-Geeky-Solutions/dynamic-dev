using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class BusBookingValidationService : IBookingValidationService<BusBooking,Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly ILogger<BusBookingValidationService> _logger;
        public BusBookingValidationService(IApplicationUnitOfWork applicationUnitOfWork,ILogger<BusBookingValidationService> logger)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _logger = logger;
        }
        public async Task<bool> IsExist(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _logger.LogWarning("Invalid booking ID provided for existence check.");
                    throw new ArgumentException("booking ID cannot be empty.", nameof(id));
                }
                var exists = false;
                _logger.LogInformation("Checking if booking with ID: {BookingId} exists.", id);

                // Assuming the repository has an async method to check existence
                // If it doesn't, you can adapt this to use GetById instead
                

                _logger.LogInformation("booking with ID: {BookingId} {ExistenceStatus}.", id, exists ? "exists" : "does not exist");
                return await _applicationUnitOfWork.BusBookingRepository.GetByIdAsync(id) != null;

            }
            catch (Exception ex) when (ex is not ArgumentException) // Don't catch ArgumentException that we throw ourselves
            {
                _logger.LogError(ex, "An error occurred while checking if booking with ID: {PookingId} exists.", id);
                throw new InvalidOperationException($"An error occurred while checking if the booking with ID: {id} exists.", ex);
            }
        }
    }
}
