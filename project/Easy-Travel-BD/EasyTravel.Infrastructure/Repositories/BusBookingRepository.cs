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
    public class BusBookingRepository : Repository<BusBooking, Guid>, IBusBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BusBookingRepository(ApplicationDbContext context) : base(context)
        {

            _context = context;
        }

    public void DeleteBusBooking(Guid Id)
{
    var busbooking = _context.BusBookings.FirstOrDefault(x => x.Id == Id);
    if (busbooking != null)
    {
        _context.BusBookings.Remove(busbooking);
        _context.SaveChanges();
    }
}


        public IEnumerable<BusBooking> GetAllBusBookings()
        {
            return _context.BusBookings
                .Include(b => b.Bus)
                //.Include(b => b.User)
                .ToList();
        }
    }
}
