using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class CarBooking : IEntity<Guid>
    {
        [Key, ForeignKey("Booking")]
        public Guid Id { get; set; }

        [Required]
        public Guid CarId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string PassengerName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        public DateTime BookingDate { get; set; }

        public Car? Car { get; set; }
        public Booking? Booking { get; set; }

    }

}

