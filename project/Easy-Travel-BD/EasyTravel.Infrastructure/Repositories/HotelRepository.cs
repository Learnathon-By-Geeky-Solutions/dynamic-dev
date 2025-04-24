using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure.Repositories
{
    public class HotelRepository : Repository<Hotel, Guid>, IHotelRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRepository(ApplicationDbContext context) : base(context)
        {
            _context=context;
        }

        public IEnumerable<Hotel> GetHotels(string location, DateTime? travelDateTime)
        {
            var query = _context.Hotels
                         .Include(h => h.Rooms)
                         .Where(h => h.City.Contains(location) || h.Name.Contains(location));
            if (travelDateTime.HasValue)
            {
                query = query.Where(h => h.Rooms.Any(r => r.IsAvailable && r.CreatedAt <= travelDateTime.Value));
            }
            return query.ToList();
        }

        // NO NEED GetRooms but i will implement it later.
        public IEnumerable<Room> GetRooms()
        {
            return _context.Rooms;
        }

        // Recommendition
        public IEnumerable<Hotel> GetTopRatedAsync(int cnt = 5)
        {
            var query = _context.Hotels.Where(h => h.Rating >= 4);
            return query.ToList();
        }

    }
}
