using EasyTravel.Domain.Interfaces;
using EasyTravel.Domain.Models;
using EasyTravel.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Infrastructure.Repositories
{
    public class BusRepository : IBusRepository
    {

        private readonly ApplicationDbContext _context;

        public BusRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public void Addbus(Bus bus)
        {
         

            _context.Buses.Add(bus);
            _context.SaveChanges();


        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return _context.Buses.ToList();
        }

        public Bus GetBusbyId(int busServiceId)
        {
            var bus = _context.Buses.Find(busServiceId);
            if (bus == null)
            {
                throw new KeyNotFoundException($"Bus with ID {busServiceId} was not found.");
            }
            return bus;
        }
    }
}