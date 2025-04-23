using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class GuideBooking : IEntity<Guid>
    {
        [Key, ForeignKey("Booking")]
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Gender { get; set; }
        public string? EventType { get; set; }
        public string? EventLocation { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TimeInHour { get; set; }
        public Guid GuideId { get; set; }
        public Guide? Guide { get; set; }
        public Booking? Booking { get; set; }
    }
}
