using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class BusBooking
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BusId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string PassengerName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public List<Seat> SelectedSeats { get; set; }

        public Bus Bus { get; set; }




    }
}
