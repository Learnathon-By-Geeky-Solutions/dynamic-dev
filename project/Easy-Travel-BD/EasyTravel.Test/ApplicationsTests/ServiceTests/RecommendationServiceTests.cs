using EasyTravel.Application.Services;
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyTravel.Test.ApplicationTests.Services
{
    [TestFixture]
    public class RecommendationServiceTests
    {
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock = null!;
        private Mock<IRecommendationRepository> _recommendationRepositoryMock = null!;
        private Mock<ILogger<RecommendationService>> _loggerMock = null!;
        private RecommendationService _recommendationService = null!;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IApplicationUnitOfWork>();
            _recommendationRepositoryMock = new Mock<IRecommendationRepository>();
            _loggerMock = new Mock<ILogger<RecommendationService>>();

            _unitOfWorkMock.Setup(u => u.RecommendationRepository).Returns(_recommendationRepositoryMock.Object);

            _recommendationService = new RecommendationService(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Test]
        public void GetRecommendationsAsync_ShouldThrowArgumentException_WhenTypeIsNullOrEmpty()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _recommendationService.GetRecommendationsAsync(null!));
            Assert.That(ex!.ParamName, Is.EqualTo("type"));

            ex = Assert.ThrowsAsync<ArgumentException>(() => _recommendationService.GetRecommendationsAsync(string.Empty));
            Assert.That(ex!.ParamName, Is.EqualTo("type"));

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Invalid recommendation type provided.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Exactly(2));
        }

        [Test]
        public void GetRecommendationsAsync_ShouldThrowArgumentOutOfRangeException_WhenCountIsZeroOrNegative()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _recommendationService.GetRecommendationsAsync("type", 0));
            Assert.That(ex!.ParamName, Is.EqualTo("count"));

            ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _recommendationService.GetRecommendationsAsync("type", -1));
            Assert.That(ex!.ParamName, Is.EqualTo("count"));

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Invalid count provided for recommendations")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Exactly(2));
        }

        [Test]
        public async Task GetRecommendationsAsync_ShouldReturnRecommendations_WhenValidInputsAreProvided()
        {
            // Arrange
            var type = "hotel";
            var count = 5;
            var recommendations = new List<RecommendationDto>
            {
                new RecommendationDto { Title = "Hotel A", Description = "Description A", Rating = 4.5 },
                new RecommendationDto { Title = "Hotel B", Description = "Description B", Rating = 4.0 }
            };

            _recommendationRepositoryMock.Setup(r => r.GetRecommendationsAsync(type, count))
                .ReturnsAsync(recommendations);

            // Act
            var result = await _recommendationService.GetRecommendationsAsync(type, count);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(result.First().Title, Is.EqualTo("Hotel A"));
            });

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Fetching {count} recommendations of type: {type}")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Successfully fetched {count} recommendations of type: {type}")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public void GetRecommendationsAsync_ShouldThrowInvalidOperationException_WhenExceptionOccurs()
        {
            // Arrange
            var type = "hotel";
            var count = 5;

            _recommendationRepositoryMock.Setup(r => r.GetRecommendationsAsync(type, count))
                .ThrowsAsync(new Exception("Database connection failed."));

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _recommendationService.GetRecommendationsAsync(type, count));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex!.Message, Is.EqualTo($"An error occurred while fetching recommendations of type: {type}."));
                Assert.That(ex.InnerException, Is.TypeOf<Exception>());
                Assert.That(ex.InnerException?.Message, Is.EqualTo("Database connection failed."));
            });

            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"An error occurred while fetching recommendations of type: {type}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}


