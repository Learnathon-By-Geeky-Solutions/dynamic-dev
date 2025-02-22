using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure.Repositories
{
    public class HotelBookingRepository : Repository<HotelBooking,Guid>,IHotelBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelBookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
