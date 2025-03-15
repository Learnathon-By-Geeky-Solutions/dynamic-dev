using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class PhotographerBookingViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TimeInHour { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PhotographerName { get; set; }
        public string? PhotographerEmail { get; set; }
        public string? PhotographerPhoneNumber { get; set; }

    }
}
