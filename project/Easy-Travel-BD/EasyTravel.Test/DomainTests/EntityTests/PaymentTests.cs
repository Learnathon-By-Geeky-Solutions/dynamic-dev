using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Enums;
using NUnit.Framework;
using System;

namespace EasyTravel.Test.DomainTests.EntityTests
{
    [TestFixture]
    public class PaymentTests
    {
        [Test]
        public void Payment_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                PaymentMethod = PaymentMethods.SSLCommerz,
                Amount = 100.00m,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Completed,
                BookingId = Guid.NewGuid(),
                TransactionId = Guid.NewGuid()
            };

            // Assert
            Assert.Multiple(() =>
            {
            Assert.That(payment.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(payment.PaymentMethod, Is.EqualTo(PaymentMethods.SSLCommerz));
            Assert.That(payment.Amount, Is.EqualTo(100.00m));
            Assert.That(payment.PaymentDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(payment.PaymentStatus, Is.EqualTo(PaymentStatus.Completed));
            Assert.That(payment.BookingId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(payment.TransactionId, Is.Not.EqualTo(Guid.Empty));
            });
            
        }
    }
}
