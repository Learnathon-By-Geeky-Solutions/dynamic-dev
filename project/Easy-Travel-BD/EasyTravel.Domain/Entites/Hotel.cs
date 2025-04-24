using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class Hotel : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(200)]
        public required string Address { get; set; }

        public required string Description { get; set; }

        [Required]
        [StringLength(50)]
        public required string City { get; set; }

        [Phone]
        public required string Phone { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } = 3;

        public required string Image { get; set; }

        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 

        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        // Add this navigation property
        public ICollection<HotelBooking> HotelBookings { get; set; } = new List<HotelBooking>();

    }
}
