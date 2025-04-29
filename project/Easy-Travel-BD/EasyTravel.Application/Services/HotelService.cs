using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<HotelService> _logger;

        public HotelService(IApplicationUnitOfWork unitOfWork, ILogger<HotelService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Create(Hotel entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to create a null Hotel entity.");
                throw new ArgumentNullException(nameof(entity), "Hotel entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating a new hotel with ID: {Id}", entity.Id);
                _unitOfWork.HotelRepository.Add(entity);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully created hotel with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the hotel with ID: {Id}", entity.Id);
                throw new InvalidOperationException($"An error occurred while creating the hotel with ID: {entity.Id}.", ex);
            }
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid hotel ID provided for deletion.");
                throw new ArgumentException("Hotel ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Deleting hotel with ID: {Id}", id);
                var hotel = _unitOfWork.HotelRepository.GetById(id);
                if (hotel == null)
                {
                    _logger.LogWarning("Hotel with ID: {Id} not found for deletion.", id);
                    throw new KeyNotFoundException($"Hotel with ID: {id} not found.");
                }

                _unitOfWork.HotelRepository.Remove(hotel);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted hotel with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the hotel with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the hotel with ID: {id}.", ex);
            }
        }

        public Hotel Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid hotel ID provided for retrieval.");
                throw new ArgumentException("Hotel ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching hotel with ID: {Id}", id);
                var hotel = _unitOfWork.HotelRepository.GetById(id);
                return hotel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the hotel with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the hotel with ID: {id}.", ex);
            }
        }

        public IEnumerable<Hotel> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all hotels.");
                return _unitOfWork.HotelRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all hotels.");
                throw new InvalidOperationException("An error occurred while fetching all hotels.", ex);
            }
        }

        public void Update(Hotel entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to update a null Hotel entity.");
                throw new ArgumentNullException(nameof(entity), "Hotel entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating hotel with ID: {Id}", entity.Id);
                _unitOfWork.HotelRepository.Edit(entity);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully updated hotel with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the hotel with ID: {Id}", entity.Id);
                throw new InvalidOperationException($"An error occurred while updating the hotel with ID: {entity.Id}.", ex);
            }
        }

        public IEnumerable<Hotel> SearchHotels(string location, DateTime? value)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                _logger.LogWarning("Invalid location provided for searching hotels.");
                throw new ArgumentException("Location cannot be empty.", nameof(location));
            }

            try
            {
                _logger.LogInformation("Searching hotels in location: {Location} with travel date: {Date}", location, value);
                return _unitOfWork.HotelRepository.GetHotels(location, value);
            }
            catch (Exception ex)
            {
                var sanitizedLocation = location.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                _logger.LogError(ex, "An error occurred while searching hotels in location: {Location} with travel date: {Date}", sanitizedLocation, value);
                throw new InvalidOperationException($"An error occurred while searching hotels in location: {sanitizedLocation} with travel date: {value}.", ex);
            }
        }

        public async Task<PagedResult<Hotel>> GetPaginatedHotelsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning("Invalid pagination parameters. Page number: {PageNumber}, Page size: {PageSize}", pageNumber, pageSize);
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            try
            {
                _logger.LogInformation("Fetching paginated hotels for page {PageNumber} with size {PageSize}.", pageNumber, pageSize);

                // Fetch total count of hotels
                var totalItems = await _unitOfWork.HotelRepository.GetCountAsync();

                // Fetch paginated hotels
                var hotels = await _unitOfWork.HotelRepository.GetAllAsync();
                var paginatedHotels = hotels
                    .OrderBy(h => h.Name) // Sort by hotel name
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Create the paginated result
                var result = new PagedResult<Hotel>
                {
                    Items = paginatedHotels,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };

                _logger.LogInformation("Successfully fetched {Count} hotels for page {PageNumber}.", paginatedHotels.Count, pageNumber);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated hotels for page {PageNumber} with size {PageSize}.", pageNumber, pageSize);
                throw new InvalidOperationException("An error occurred while fetching paginated hotels.", ex);
            }
        }
    }
}