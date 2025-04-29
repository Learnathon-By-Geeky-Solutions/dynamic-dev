using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;

namespace EasyTravel.Test.DomainTests.EntityTests
{
    [TestFixture]
    public class PhotographerBookingTests
    {
        [Test]
        public void PhotographerBooking_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var booking = new PhotographerBooking
            {
                Id = Guid.NewGuid(),
                UserName = "John Doe",
                Email = "john.doe@example.com",
                Gender = "Male",
                EventType = "Wedding",
                EventLocation = "New York",
                EventDate = DateTime.UtcNow,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(12),
                TimeInHour = 2,
                PhotographerId = Guid.NewGuid()
            };

            // Assert
            Assert.Multiple(() =>
            {
            Assert.That(booking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(booking.UserName, Is.EqualTo("John Doe"));
            Assert.That(booking.Email, Is.EqualTo("john.doe@example.com"));
            Assert.That(booking.Gender, Is.EqualTo("Male"));
            Assert.That(booking.EventType, Is.EqualTo("Wedding"));
            Assert.That(booking.EventLocation, Is.EqualTo("New York"));
            Assert.That(booking.EventDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(booking.StartTime, Is.EqualTo(TimeSpan.FromHours(10)));
            Assert.That(booking.EndTime, Is.EqualTo(TimeSpan.FromHours(12)));
            Assert.That(booking.TimeInHour, Is.EqualTo(2));
            Assert.That(booking.PhotographerId, Is.Not.EqualTo(Guid.Empty));
            }); 
            }
    }
}
