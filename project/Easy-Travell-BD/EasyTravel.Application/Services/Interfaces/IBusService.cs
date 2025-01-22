using EasyTravel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services.Interfaces
{
    public interface IBusService
    {

        void CreateBus(Bus bus);
        IEnumerable<Bus> GetAllBuses();


    }
}
