using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Factories
{
    public interface IBookingManager
    {
         IBookingFactory GetBooking(string type);
    }
}
