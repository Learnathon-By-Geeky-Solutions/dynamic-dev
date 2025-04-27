using EasyTravel.Domain.Entites;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTravel.Test.InfrastructureTests.RepositoryTests
{
    [TestFixture]
    public class GuideRepositoryTests
    {
        private ApplicationDbContext _context;
        private GuideRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Guide")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new GuideRepository(_context);

            // Seed data
            _context.Guides.AddRange(new List<Guide>
            {
                new Guide
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    ContactNumber = "123-456-7890",
                    Address = "123 Main St",
                    ProfilePicture = "profile1.jpg",
                    Bio = "Experienced guide",
                    DateOfBirth = new DateTime(1985, 1, 1),
                    LanguagesSpoken = "English, Spanish",
                    PreferredLocations = "City A, City B",
                    PreferredEvents = "City Tours",
                    Specialization = "History",
                    YearsOfExperience = 10,
                    LicenseNumber = "LICENSE123",
                    Availability = true,
                    HourlyRate = 50.00m,
                    Rating = 4.5m,
                    AgencyId = Guid.NewGuid()
                },
                new Guide
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    ContactNumber = "987-654-3210",
                    Address = "456 Elm St",
                    ProfilePicture = "profile2.jpg",
                    Bio = "Nature guide",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    LanguagesSpoken = "English, French",
                    PreferredLocations = "City C, City D",
                    PreferredEvents = "Nature Tours",
                    Specialization = "Wildlife",
                    YearsOfExperience = 5,
                    LicenseNumber = "LICENSE456",
                    Availability = true,
                    HourlyRate = 40.00m,
                    Rating = 4.8m,
                    AgencyId = Guid.NewGuid()
                }
            });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

       
        [Test]
        public void GetAllGuides_ShouldReturnAllGuides()
        {
            var result = _repository.GetAll().ToList(); // Convert to a list for safe access

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2), "The count of guides is incorrect."); // Updated
                Assert.That(result[0].FirstName, Is.EqualTo("John"), "The first guide's name is incorrect.");
            });
        }

        [Test]
        public void AddGuide_ShouldAddGuide()
        {
            var guide = new Guide
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                ContactNumber = "555-123-4567",
                Address = "789 Oak St",
                ProfilePicture = "profile3.jpg",
                Bio = "Adventure guide",
                DateOfBirth = new DateTime(1995, 3, 10),
                LanguagesSpoken = "English, German",
                PreferredLocations = "City E, City F",
                PreferredEvents = "Adventure Tours",
                Specialization = "Mountaineering",
                YearsOfExperience = 7,
                LicenseNumber = "LICENSE789",
                Availability = true,
                HourlyRate = 60.00m,
                Rating = 4.9m,
                AgencyId = Guid.NewGuid()
            };

            _repository.Add(guide);
            _context.SaveChanges();
            var result = _repository.GetAll().ToList(); // Convert to a list for safe access

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3), "The count of guides is incorrect."); // Updated
                Assert.That(result.Any(g => g.FirstName == "Alice"), Is.True, "The guide 'Alice' was not found.");
            });
        }
    }
}