using EasyTravel.Application.Services;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EasyTravel.Test.ApplicationTests.Services
{
    [TestFixture]
    public class PhotographerServiceTests
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock = null!;
        private Mock<ILogger<PhotographerService>> _loggerMock = null!;
        private PhotographerService _photographerService = null!;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _loggerMock = new Mock<ILogger<PhotographerService>>();
            _photographerService = new PhotographerService(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Get_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _photographerService.Get(Guid.Empty));
            Assert.That(ex!.ParamName, Is.EqualTo("id"));

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Invalid photographer ID provided for retrieval.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public void Get_ShouldReturnPhotographer_WhenPhotographerExists()
        {
            // Arrange
            var photographerId = Guid.NewGuid();
            var photographer = new Photographer
            {
                Id = photographerId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                ContactNumber = "123-456-7890",
                PreferredLocations = "CityA",
                PreferredEvents = "Wedding",
                Address = "123 Main St",
                ProfilePicture = "profile.jpg",
                Bio = "Experienced photographer",
                DateOfBirth = DateTime.Now.AddYears(-30),
                Skills = "Photography",
                PortfolioUrl = "http://portfolio.com",
                Specialization = "Wedding",
                YearsOfExperience = 5,
                Availability = true,
                HourlyRate = 100,
                Rating = 4.5m,
                HireDate = DateTime.Now.AddYears(-2),
                UpdatedAt = DateTime.Now,
                AgencyId = Guid.NewGuid()
            };

            _unitOfWorkMock.Setup(u => u.PhotographerRepository.GetById(photographerId)).Returns(photographer);

            // Act
            var result = _photographerService.Get(photographerId);

            // Assert
            Assert.That(result, Is.EqualTo(photographer));
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Fetching photographer with ID: {photographerId}")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllPhotographers()
        {
            // Arrange
            var photographers = new List<Photographer>
            {
                new Photographer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    ContactNumber = "123-456-7890",
                    PreferredLocations = "CityA",
                    PreferredEvents = "Wedding",
                    Address = "123 Main St",
                    ProfilePicture = "profile.jpg",
                    Bio = "Experienced photographer",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Skills = "Photography",
                    PortfolioUrl = "http://portfolio.com",
                    Specialization = "Wedding",
                    YearsOfExperience = 5,
                    Availability = true,
                    HourlyRate = 100,
                    Rating = 4.5m,
                    HireDate = DateTime.Now.AddYears(-2),
                    UpdatedAt = DateTime.Now,
                    AgencyId = Guid.NewGuid()
                },
                new Photographer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    ContactNumber = "987-654-3210",
                    PreferredLocations = "CityB",
                    PreferredEvents = "Portrait",
                    Address = "456 Elm St",
                    ProfilePicture = "profile2.jpg",
                    Bio = "Creative photographer",
                    DateOfBirth = DateTime.Now.AddYears(-25),
                    Skills = "Portrait Photography",
                    PortfolioUrl = "http://portfolio2.com",
                    Specialization = "Portrait",
                    YearsOfExperience = 3,
                    Availability = true,
                    HourlyRate = 80,
                    Rating = 4.8m,
                    HireDate = DateTime.Now.AddYears(-1),
                    UpdatedAt = DateTime.Now,
                    AgencyId = Guid.NewGuid()
                }
            };

            _unitOfWorkMock.Setup(u => u.PhotographerRepository.GetAll()).Returns(photographers);

            // Act
            var result = _photographerService.GetAll();

            // Assert
            Assert.That(result, Is.EqualTo(photographers));
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Fetching all photographers.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public void GetPhotographerListAsync_ShouldThrowArgumentNullException_WhenPhotographerBookingIsNull()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _photographerService.GetPhotographerListAsync(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("photographerBooking"));

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Attempted to fetch photographer list with a null PhotographerBooking model.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public async Task GetPhotographerListAsync_ShouldReturnPhotographers_WhenValidInputsAreProvided()
        {
            // Arrange
            var photographerBooking = new PhotographerBooking
            {
                Id = Guid.NewGuid(),
                UserName = "John Doe",
                Email = "john.doe@example.com",
                Gender = "Male",
                EventDate = DateTime.Now.AddDays(1),
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(12),
                PhotographerId = Guid.NewGuid()
            };

            var photographers = new List<Photographer>
    {
        new Photographer
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            ContactNumber = "123-456-7890",
            PreferredLocations = "CityA",
            PreferredEvents = "Wedding",
            Address = "123 Main St",
            ProfilePicture = "profile.jpg",
            Bio = "Experienced photographer",
            DateOfBirth = DateTime.Now.AddYears(-30),
            Skills = "Photography",
            PortfolioUrl = "http://portfolio.com",
            Specialization = "Wedding",
            YearsOfExperience = 5,
            Availability = true,
            HourlyRate = 100,
            Rating = 4.5m,
            HireDate = DateTime.Now.AddYears(-2),
            UpdatedAt = DateTime.Now,
            AgencyId = Guid.NewGuid()
        },
        new Photographer
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            ContactNumber = "987-654-3210",
            PreferredLocations = "CityB",
            PreferredEvents = "Portrait",
            Address = "456 Elm St",
            ProfilePicture = "profile2.jpg",
            Bio = "Creative photographer",
            DateOfBirth = DateTime.Now.AddYears(-25),
            Skills = "Portrait Photography",
            PortfolioUrl = "http://portfolio2.com",
            Specialization = "Portrait",
            YearsOfExperience = 3,
            Availability = true,
            HourlyRate = 80,
            Rating = 4.8m,
            HireDate = DateTime.Now.AddYears(-1),
            UpdatedAt = DateTime.Now,
            AgencyId = Guid.NewGuid()
        }
    };

            _unitOfWorkMock.Setup(u => u.PhotographerRepository.GetAsync(It.IsAny<Expression<Func<Photographer, bool>>>(), null))
                .ReturnsAsync(photographers);

            // Act
            var result = await _photographerService.GetPhotographerListAsync(photographerBooking);

            // Assert
            Assert.That(result, Is.EqualTo(photographers));

            var expectedLogMessage = $"Fetching photographer list for event on {photographerBooking.EventDate:MM/dd/yyyy HH:mm:ss} at {photographerBooking.StartTime}";
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedLogMessage)),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

    }
}





