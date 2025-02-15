using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class Seat
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BusId { get; set; }
        [Required]
        [MaxLength(5)]
        public required string SeatNumber { get; set; } // E.g., "A1", "B2"

        [Required]
        public bool IsAvailable { get; set; } // E.g., true for available, false for sold
        public Bus Bus { get; set; }



    }
}
