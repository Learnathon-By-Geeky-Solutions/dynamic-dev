using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IApplicationUnitOfWork unitOfWork, ILogger<RoomService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Create(Room entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to create a null Room entity.");
                throw new ArgumentNullException(nameof(entity), "Room entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating a new room with ID: {Id}", entity.Id);
                _unitOfWork.RoomRepository.Add(entity);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully created room with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the room with ID: {Id}", entity.Id);
                throw new InvalidOperationException($"An error occurred while creating the room with ID: {entity.Id}.", ex);
            }
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid room ID provided for deletion.");
                throw new ArgumentException("Room ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Deleting room with ID: {Id}", id);
                var room = _unitOfWork.RoomRepository.GetById(id);
                if (room == null)
                {
                    _logger.LogWarning("Room with ID: {Id} not found for deletion.", id);
                    throw new KeyNotFoundException($"Room with ID: {id} not found.");
                }

                _unitOfWork.RoomRepository.Remove(room);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted room with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the room with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the room with ID: {id}.", ex);
            }
        }

        public Room Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid room ID provided for retrieval.");
                throw new ArgumentException("Room ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching room with ID: {Id}", id);
                var room = _unitOfWork.RoomRepository.GetById(id);
                return room;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the room with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the room with ID: {id}.", ex);
            }
        }

        public IEnumerable<Room> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all rooms.");
                return _unitOfWork.RoomRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all rooms.");
                throw new InvalidOperationException("An error occurred while fetching all rooms.", ex);
            }
        }

        public void Update(Room entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to update a null Room entity.");
                throw new ArgumentNullException(nameof(entity), "Room entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating room with ID: {Id}", entity.Id);
                _unitOfWork.RoomRepository.Edit(entity);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully updated room with ID: {Id}", entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the room with ID: {Id}", entity.Id);
                throw new InvalidOperationException($"An error occurred while updating the room with ID: {entity.Id}.", ex);
            }
        }

        public async Task<IEnumerable<Room>> GetRoomByHotel(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid hotel ID provided for fetching rooms.");
                throw new ArgumentException("Hotel ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching rooms for hotel with ID: {HotelId}", id);
                var rooms = await _unitOfWork.RoomRepository.GetAsync(e => e.HotelId == id);
                _logger.LogInformation("Successfully fetched rooms for hotel with ID: {HotelId}", id);
                return rooms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching rooms for hotel with ID: {HotelId}", id);
                throw new InvalidOperationException($"An error occurred while fetching rooms for hotel with ID: {id}.", ex);
            }
        }

        public async Task<PagedResult<Room>> GetPaginatedRoomsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning("Invalid pagination parameters. Page number: {PageNumber}, Page size: {PageSize}", pageNumber, pageSize);
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            try
            {
                _logger.LogInformation("Fetching paginated rooms for page {PageNumber} with size {PageSize}.", pageNumber, pageSize);

                // Fetch total count of rooms
                var totalItems = await _unitOfWork.RoomRepository.GetCountAsync();

                // Fetch paginated rooms
                var rooms = await _unitOfWork.RoomRepository.GetAllAsync();
                var paginatedRooms = rooms
                    .OrderBy(r => r.RoomNumber) // Sort by room number
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Create the paginated result
                var result = new PagedResult<Room>
                {
                    Items = paginatedRooms,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };

                _logger.LogInformation("Successfully fetched {Count} rooms for page {PageNumber}.", paginatedRooms.Count, pageNumber);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated rooms for page {PageNumber} with size {PageSize}.", pageNumber, pageSize);
                throw new InvalidOperationException("An error occurred while fetching paginated rooms.", ex);
            }
        }
    }
}