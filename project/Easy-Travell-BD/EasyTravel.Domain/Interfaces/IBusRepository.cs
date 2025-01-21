using EasyTravel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Interfaces
{
     public interface IBusRepository
    {
        void Addbus(Bus bus);
        IEnumerable<Bus> GetAllBuses();
        Bus GetBusbyId(int busServiceId);



    }
}
