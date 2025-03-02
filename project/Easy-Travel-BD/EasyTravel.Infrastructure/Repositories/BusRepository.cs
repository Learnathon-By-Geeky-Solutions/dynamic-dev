using EasyTravel.Domain.Entites;
using EasyTravel.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyTravel.Domain.Repositories;

namespace EasyTravel.Infrastructure.Repositories
{
    public class BusRepository : Repository<Bus, Guid>, IBusRepository
    {
        private readonly ApplicationDbContext _context;

        public BusRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public void Addbus(Bus bus)
        {
            _context.Buses.Add(bus);
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return _context.Buses.ToList();
        }
    }
}
