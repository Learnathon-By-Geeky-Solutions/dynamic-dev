using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<RecommendationService> _logger;

        public RecommendationService(IApplicationUnitOfWork unitOfWork, ILogger<RecommendationService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RecommendationDto>> GetRecommendationsAsync(string type, int count = 5)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                _logger.LogWarning("Invalid recommendation type provided.");
                throw new ArgumentException("Recommendation type cannot be null or empty.", nameof(type));
            }

            if (count <= 0)
            {
                _logger.LogWarning("Invalid count provided for recommendations: {Count}", count);
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            try
            {
                var sanitizedType = type.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                _logger.LogInformation("Fetching {Count} recommendations of type: {Type}", count, sanitizedType);
                var recommendations = await _unitOfWork.RecommendationRepository.GetRecommendationsAsync(type, count);
                _logger.LogInformation("Successfully fetched {Count} recommendations of type: {Type}", count, sanitizedType);
                return recommendations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching recommendations of type: {Type}", type);
                throw new InvalidOperationException($"An error occurred while fetching recommendations of type: {type}.", ex);
            }
        }

        public void GetRecommendation(string type, int count = 5)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                _logger.LogWarning("Invalid recommendation type provided.");
                throw new ArgumentException("Recommendation type cannot be null or empty.", nameof(type));
            }

            if (count <= 0)
            {
                _logger.LogWarning("Invalid count provided for recommendations: {Count}", count);
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            try
            {
                var sanitizedType = type.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                _logger.LogInformation("Fetching {Count} recommendations of type: {Type}", count, sanitizedType);
                _unitOfWork.RecommendationRepository.GetRecommendationsAsync(type, count);
                _logger.LogInformation("Successfully fetched {Count} recommendations of type: {Type}", count, sanitizedType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching recommendations of type: {Type}", type);
                throw new InvalidOperationException($"An error occurred while fetching recommendations of type: {type}.", ex);
            }
        }
    }
}