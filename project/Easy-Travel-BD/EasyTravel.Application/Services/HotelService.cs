﻿using EasyTravel.Domain;
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

        public PagedResult<Hotel> GetAllPaginatedHotels(int pageNumber,int pageSize)
        {
            try
            {
                _logger.LogInformation("Fetching all hotels.");
                var totalItems = _unitOfWork.HotelRepository.GetCount();
                var paginateHotels = _unitOfWork.HotelRepository.GetAll().
                    OrderBy(h => h.Name).
                    Skip((pageNumber - 1) * pageSize).
                    Take(pageSize).
                    ToList();
                var result = new PagedResult<Hotel>
                {
                    Items = paginateHotels,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };
                return result;
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

        public PagedResult<Hotel> SearchHotels(string location, DateTime? value,int pageNumber,int pageSize)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                _logger.LogWarning("Invalid location provided for searching hotels.");
                throw new ArgumentException("Location cannot be empty.", nameof(location));
            }
            try
            {
                _logger.LogInformation("Searching hotels in location: {Location} with travel date: {Date}", location, value);
                 var hotels = _unitOfWork.HotelRepository.GetHotels(location, value);
                var totalItems = hotels.Count();
                var paginateHotels = hotels.
                    OrderBy(h => h.Name).
                    Skip((pageNumber - 1) * pageSize).
                    Take(pageSize).
                    ToList();
                var result = new PagedResult<Hotel>
                {
                    Items = paginateHotels,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };
                return result;

            }
            catch (Exception ex)
            {
                var sanitizedLocation = location.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                _logger.LogError(ex, "An error occurred while searching hotels in location: {Location} with travel date: {Date}", sanitizedLocation, value);
                throw new InvalidOperationException($"An error occurred while searching hotels in location: {sanitizedLocation} with travel date: {value}.", ex);
            }
        }

        public IEnumerable<Hotel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}