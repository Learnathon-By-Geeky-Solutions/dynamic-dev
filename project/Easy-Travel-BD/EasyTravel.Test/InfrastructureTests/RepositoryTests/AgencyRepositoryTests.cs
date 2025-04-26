using EasyTravel.Domain.Entites;
using EasyTravel.Infrastructure.Data;
using EasyTravel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.InMemory; // Add this using directive

namespace EasyTravel.Test.InfrastructureTests.RepositoryTests
{


    [TestFixture]
    public class AgencyRepositoryTests
    {
        private ApplicationDbContext _context;
        private AgencyRepository _repository;

        [SetUp]
        public void SetUp()
        {
            // Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Use the new constructor for testing
            _context = new ApplicationDbContext(options);
            _repository = new AgencyRepository(_context);

            // Seed data
            _context.Agencies.AddRange(new List<Agency>
            {
                new Agency
                {
                    Id = Guid.NewGuid(),
                    Name = "Agency 1",
                    Address = "123 Main St, New York, NY",
                    ContactNumber = "123-456-7890",
                    LicenseNumber = "LICENSE123"
                },
                new Agency
                {
                    Id = Guid.NewGuid(),
                    Name = "Agency 2",
                    Address = "456 Elm St, Los Angeles, CA",
                    ContactNumber = "987-654-3210",
                    LicenseNumber = "LICENSE456"
                }
            });
            _context.SaveChanges();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Add_ShouldAddAgency()
        {
            // Arrange
            var agency = new Agency
            {
                Id = Guid.NewGuid(),
                Name = "Test Agency",
                Address = "789 Oak St, Chicago, IL",
                ContactNumber = "654-321-0987",
                LicenseNumber = "LICENSE789"
            };

            // Act
            _repository.Add(agency);
            _context.SaveChanges();

            // Assert
            var result = _repository.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(3), "The count of agencies is incorrect."); // Updated
                Assert.That(result.Any(a => a.Name == "Test Agency"), Is.True, "The added agency is missing.");
            });
        }

        [Test]
        public void GetAll_ShouldReturnAllAgencies()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2), "The count of agencies is incorrect."); // Updated
                Assert.That(result[0].Name, Is.EqualTo("Agency 1"), "The first agency's name is incorrect.");
            });
        }

        [Test]
        public void Remove_ShouldRemoveAgency()
        {
            // Arrange
            var agencyToRemove = _context.Agencies.First();

            // Act
            _repository.Remove(agencyToRemove);
            _context.SaveChanges();

            // Assert
            var result = _repository.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(1), "The count of agencies is incorrect after removal."); // Updated
                Assert.That(result.Any(a => a.Id == agencyToRemove.Id), Is.False, "The removed agency still exists.");
            });
        }
    }
}