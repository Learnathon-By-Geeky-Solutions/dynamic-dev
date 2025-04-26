using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EasyTravel.Application.Services
{
    public class BusService : IBusService
    {
        private readonly ILogger<BusService> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;

        public BusService(IApplicationUnitOfWork unitOfWork, ILogger<BusService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void CreateBus(Bus bus)
        {
            if (bus == null)
            {
                _logger.LogWarning("Attempted to create a null Bus entity.");
                throw new ArgumentNullException(nameof(bus), "Bus entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating a new bus with ID: {Id}", bus.Id);

                for (char row = 'A'; row <= 'G'; row++)
                {
                    for (int col = 1; col <= 4; col++)
                    {
                        var seat = new Seat
                        {
                            Id = Guid.NewGuid(),
                            BusId = bus.Id,
                            SeatNumber = $"{row}{col}",
                            IsAvailable = true
                        };
                        bus.Seats?.Add(seat);
                    }
                }

                _unitOfWork.BusRepository.Addbus(bus);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully created bus with ID: {Id}", bus.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the bus with ID: {Id}", bus.Id);
                throw new InvalidOperationException($"An error occurred while creating the bus with ID: {bus.Id}.", ex);
            }
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            try
            {
                _logger.LogInformation("Fetching all buses.");
                return _unitOfWork.BusRepository.GetAllBuses();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all buses.");
                throw new InvalidOperationException("An error occurred while fetching all buses.", ex);
            }
        }

        public Bus GetseatBusById(Guid busId)
        {
            if (busId == Guid.Empty)
            {
                _logger.LogWarning("Invalid bus ID provided for fetching seats.");
                throw new ArgumentException("Bus ID cannot be empty.", nameof(busId));
            }

            try
            {
                _logger.LogInformation("Fetching bus with seats for ID: {Id}", busId);
                var bus = _unitOfWork.BusRepository
                    .GetBuses()
                    .Include(b => b.Seats)
                    .FirstOrDefault(b => b.Id == busId);

                return bus!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the bus with seats for ID: {Id}", busId);
                throw new InvalidOperationException($"An error occurred while fetching the bus with seats for ID: {busId}.", ex);
            }
        }

        public Bus GetBusById(Guid BusId)
        {
            if (BusId == Guid.Empty)
            {
                _logger.LogWarning("Invalid bus ID provided for retrieval.");
                throw new ArgumentException("Bus ID cannot be empty.", nameof(BusId));
            }

            try
            {
                _logger.LogInformation("Fetching bus with ID: {Id}", BusId);
                var bus = _unitOfWork.BusRepository.GetById(BusId);
                return bus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the bus with ID: {Id}", BusId);
                throw new InvalidOperationException($"An error occurred while fetching the bus with ID: {BusId}.", ex);
            }
        }

        public async Task<IEnumerable<Bus>> GetAvailableBusesAsync(string from, string to, DateTime dateTime)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            {
                _logger.LogWarning("Invalid 'from' or 'to' location provided for fetching available buses.");
                throw new ArgumentException("From and To locations cannot be empty.");
            }

            try
            {
                _logger.LogInformation("Fetching available buses from {From} to {To} on {Date}", from, to, dateTime);
                var buses = await _unitOfWork.BusRepository.GetAsync(bus =>
                    bus.From == from &&
                    bus.To == to &&
                    bus.DepartureTime.Date == dateTime.Date &&
                    bus.Seats!.Any(seat => seat.IsAvailable));

                return buses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching available buses from {From} to {To} on {Date}", from, to, dateTime);
                throw new InvalidOperationException($"An error occurred while fetching available buses from {from} to {to} on {dateTime}.", ex);
            }
        }

        public void UpdateBus(Bus bus)
        {
            if (bus == null)
            {
                _logger.LogWarning("Attempted to update a null Bus entity.");
                throw new ArgumentNullException(nameof(bus), "Bus entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating bus with ID: {Id}", bus.Id);
                _unitOfWork.BusRepository.Edit(bus);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully updated bus with ID: {Id}", bus.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the bus with ID: {Id}", bus.Id);
                throw new InvalidOperationException($"An error occurred while updating the bus with ID: {bus.Id}.", ex);
            }
        }

        public void DeleteBus(Bus bus)
        {
            if (bus == null)
            {
                _logger.LogWarning("Attempted to delete a null Bus entity.");
                throw new ArgumentNullException(nameof(bus), "Bus entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Deleting bus with ID: {Id}", bus.Id);
                _unitOfWork.BusRepository.Remove(bus);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted bus with ID: {Id}", bus.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the bus with ID: {Id}", bus.Id);
                throw new InvalidOperationException($"An error occurred while deleting the bus with ID: {bus.Id}.", ex);
            }
        }

        public void SaveBooking(BusBooking model, List<Guid> seatIds, Booking booking, Payment? payment = null)
        {
            if (model == null || booking == null || seatIds == null || seatIds.Count == 0)
            {
                _logger.LogWarning("Invalid input provided for saving booking.");
                throw new ArgumentException("Model, booking, and seat IDs cannot be null or empty.");
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    _logger.LogInformation("Saving booking for bus with ID: {BusId}", model.BusId);

                    // Save the booking
                    _unitOfWork.BookingRepository.Add(booking);
                    _unitOfWork.Save();
                    _unitOfWork.BusBookingRepository.Add(model);
                    _unitOfWork.Save();

                    // Update the seat availability
                    foreach (var seatId in seatIds)
                    {
                        var seat = _unitOfWork.SeatRepository.GetById(seatId);
                        if (seat != null)
                        {
                            seat.IsAvailable = false;
                            _unitOfWork.SeatRepository.Edit(seat);
                            _unitOfWork.Save();
                        }
                    }

                    // Save payment if provided
                    if (payment != null)
                    {
                        _unitOfWork.PaymentRepository.Add(payment);
                        _unitOfWork.Save();
                    }

                    // Commit the transaction
                    transaction.Complete();
                    _logger.LogInformation("Successfully saved booking for bus with ID: {BusId}", model.BusId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while saving the booking for bus with ID: {BusId}", model.BusId);
                    throw new InvalidOperationException($"An error occurred while saving the booking for bus with ID: {model.BusId}.", ex);
                }
            }
        }
    }
}



