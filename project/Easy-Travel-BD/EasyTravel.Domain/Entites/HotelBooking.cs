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
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string RoomIdsJson { get; set; }

        [NotMapped]
        public List<Guid> RoomIds
        {
            get => string.IsNullOrEmpty(RoomIdsJson) ? new List<Guid>() : JsonSerializer.Deserialize<List<Guid>>(RoomIdsJson);
            set => RoomIdsJson = JsonSerializer.Serialize(value);
        }
    }
}
