using EasyTravel.Domain.Entites;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTest.Entities
{
    [TestFixture]
    public class PhotographerTests
    {
        [Test]
        public void Photographer_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var photographer = new Photographer
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                ContactNumber = "123-456-7890",
                PreferredLocations = "New York, Los Angeles",
                PreferredEvents = "Weddings, Birthdays",
                Address = "123 Main St, New York, NY",
                ProfilePicture = "profile.jpg",
                Bio = "Experienced photographer specializing in events.",
                DateOfBirth = new DateTime(1990, 1, 1),
                Skills = "Portrait, Landscape",
                PortfolioUrl = "http://portfolio.com/johndoe",
                Specialization = "Event Photography",
                YearsOfExperience = 10,
                Availability = true,
                HourlyRate = 100.50m,
                Rating = 4.8m,
                HireDate = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SocialMediaLinks = "http://instagram.com/johndoe",
                Status = "Active",
                AgencyId = Guid.NewGuid(),
                Agencies = new List<Agency>(),
                PhotographerBookings = new List<PhotographerBooking>()
            };

            // Assert
            Assert.Multiple(() =>
            {
                
         
            Assert.That(photographer.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(photographer.FirstName, Is.EqualTo("John"));
            Assert.That(photographer.LastName, Is.EqualTo("Doe"));
            Assert.That(photographer.Email, Is.EqualTo("john.doe@example.com"));
            Assert.That(photographer.ContactNumber, Is.EqualTo("123-456-7890"));
            Assert.That(photographer.PreferredLocations, Is.EqualTo("New York, Los Angeles"));
            Assert.That(photographer.PreferredEvents, Is.EqualTo("Weddings, Birthdays"));
            Assert.That(photographer.Address, Is.EqualTo("123 Main St, New York, NY"));
            Assert.That(photographer.ProfilePicture, Is.EqualTo("profile.jpg"));
            Assert.That(photographer.Bio, Is.EqualTo("Experienced photographer specializing in events."));
            Assert.That(photographer.DateOfBirth, Is.EqualTo(new DateTime(1990, 1, 1)));
            Assert.That(photographer.Skills, Is.EqualTo("Portrait, Landscape"));
            Assert.That(photographer.PortfolioUrl, Is.EqualTo("http://portfolio.com/johndoe"));
            Assert.That(photographer.Specialization, Is.EqualTo("Event Photography"));
            Assert.That(photographer.YearsOfExperience, Is.EqualTo(10));
            Assert.That(photographer.Availability, Is.True);
            Assert.That(photographer.HourlyRate, Is.EqualTo(100.50m));
            Assert.That(photographer.Rating, Is.EqualTo(4.8m));
            Assert.That(photographer.HireDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(photographer.UpdatedAt, Is.Not.EqualTo(default(DateTime)));
            Assert.That(photographer.SocialMediaLinks, Is.EqualTo("http://instagram.com/johndoe"));
            Assert.That(photographer.Status, Is.EqualTo("Active"));
            Assert.That(photographer.AgencyId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(photographer.Agencies, Is.Empty);
            Assert.That(photographer.PhotographerBookings, Is.Empty);
            });
        }

        [Test]
        public void Photographer_CanAddPhotographerBooking()
        {
            // Arrange
            var photographer = new Photographer
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                ContactNumber = "123-456-7890",
                PreferredLocations = "New York, Los Angeles",
                PreferredEvents = "Weddings, Birthdays",
                Address = "123 Main St, New York, NY",
                ProfilePicture = "profile.jpg",
                Bio = "Experienced photographer specializing in events.",
                DateOfBirth = new DateTime(1990, 1, 1),
                Skills = "Portrait, Landscape",
                PortfolioUrl = "http://portfolio.com/johndoe",
                Specialization = "Event Photography",
                YearsOfExperience = 10,
                Availability = true,
                HourlyRate = 100.50m,
                Rating = 4.8m,
                HireDate = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SocialMediaLinks = "http://instagram.com/johndoe",
                Status = "Active",
                AgencyId = Guid.NewGuid(),
                Agencies = new List<Agency>(),
                PhotographerBookings = new List<PhotographerBooking>()
            };

            var booking = new PhotographerBooking
            {
                Id = Guid.NewGuid(),
                UserName = "Jane Doe",
                Email = "jane.doe@example.com",
                Gender = "Female",
                EventType = "Wedding",
                EventLocation = "Los Angeles",
                EventDate = DateTime.UtcNow,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(12),
                TimeInHour = 2,
                PhotographerId = photographer.Id
            };

            // Act
            photographer.PhotographerBookings.Add(booking);

            // Assert
            Assert.That(photographer.PhotographerBookings, Has.Count.EqualTo(1));
            Assert.That(photographer.PhotographerBookings[0], Is.EqualTo(booking));
        }
    }
}

