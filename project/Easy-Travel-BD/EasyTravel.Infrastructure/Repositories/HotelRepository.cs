using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure.Repositories
{
    public class HotelRepository : Repository<Hotel, Guid>, IHotelRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
        }

        public IEnumerable<Hotel> GetAllHotel()
        {

            return _context.Hotels.ToList();
        }
    }
}
