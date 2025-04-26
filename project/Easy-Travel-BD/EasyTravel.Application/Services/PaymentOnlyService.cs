using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class PaymentOnlyService : IPaymentOnlyService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentOnlyService> _logger;

        public PaymentOnlyService(IApplicationUnitOfWork unitOfWork, ILogger<PaymentOnlyService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void AddPaymentOnly(Payment payment)
        {
            if (payment == null)
            {
                _logger.LogWarning("Attempted to add a null Payment entity.");
                throw new ArgumentNullException(nameof(payment), "Payment entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Adding a new payment with ID: {Id}", payment.Id);
                _unitOfWork.PaymentRepository.Add(payment);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully added payment with ID: {Id}", payment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the payment with ID: {Id}", payment.Id);
                throw new InvalidOperationException($"An error occurred while adding the payment with ID: {payment.Id}.", ex);
            }
        }

        public async Task<bool> IsExist(Guid bookingId)
        {
            if (bookingId == Guid.Empty)
            {
                _logger.LogWarning("Invalid booking ID provided for existence check.");
                throw new ArgumentException("Booking ID cannot be empty.", nameof(bookingId));
            }

            try
            {
                _logger.LogInformation("Checking existence of payment for booking ID: {BookingId}", bookingId);
                var paymentExists = await _unitOfWork.PaymentRepository.GetAsync(d => d.BookingId == bookingId) != null;
                _logger.LogInformation("Payment existence check for booking ID: {BookingId} returned: {Exists}", bookingId, paymentExists);
                return paymentExists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking existence of payment for booking ID: {BookingId}", bookingId);
                throw new InvalidOperationException($"An error occurred while checking existence of payment for booking ID: {bookingId}.", ex);
            }
        }
    }
}
