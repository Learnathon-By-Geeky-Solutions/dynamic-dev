using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class PhotographerBookingViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string?  Gender { get; set; }
        public string EventType { get; set; }
        public string EventLocation { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TimeInHour { get; set; }
        public decimal TotalAmount { get; set; }
        public Photographer? Photographer { get; set; }

    }
}
