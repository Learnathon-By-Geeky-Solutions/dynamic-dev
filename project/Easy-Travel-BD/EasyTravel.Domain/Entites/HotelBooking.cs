using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class HotelBooking:IEntity<Guid>
    {
        [Key, ForeignKey("Booking")]
        public Guid Id { get; set; }

        //public User User { get; set; }

        [Required]
        public Guid HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public string RoomIdsJson { get; set; }
        public Booking? Booking { get; set; }

    }
}
