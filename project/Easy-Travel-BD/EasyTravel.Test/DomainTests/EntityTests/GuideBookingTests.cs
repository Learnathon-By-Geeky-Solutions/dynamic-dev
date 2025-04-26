using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;

namespace EasyTravel.Test.DomainTests.EntityTests
{
    [TestFixture]
    public class GuideBookingTests
    {
      
        [Test]
        public void GuideBooking_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var booking = new GuideBooking
            {
                Id = Guid.NewGuid(),
                UserName = "Jane Doe",
                Email = "jane.doe@example.com",
                Gender = "Female",
                EventType = "Tour",
                EventLocation = "Paris",
                EventDate = DateTime.UtcNow,
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(17),
                TimeInHour = 8,
                GuideId = Guid.NewGuid()
            };

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(booking.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(booking.UserName, Is.EqualTo("Jane Doe"));
                Assert.That(booking.Email, Is.EqualTo("jane.doe@example.com"));
                Assert.That(booking.Gender, Is.EqualTo("Female"));
                Assert.That(booking.EventType, Is.EqualTo("Tour"));
                Assert.That(booking.EventLocation, Is.EqualTo("Paris"));
                Assert.That(booking.EventDate, Is.Not.EqualTo(default(DateTime)));
                Assert.That(booking.StartTime, Is.EqualTo(TimeSpan.FromHours(9)));
                Assert.That(booking.EndTime, Is.EqualTo(TimeSpan.FromHours(17)));
                Assert.That(booking.TimeInHour, Is.EqualTo(8));
                Assert.That(booking.GuideId, Is.Not.EqualTo(Guid.Empty));
            });
        }
        
    }
}
