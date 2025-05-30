﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class Room:IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string RoomNumber { get; set; }

        [Required]
        [StringLength(50)]
        public required string RoomType { get; set; }

        [Required]
        public decimal PricePerNight { get; set; }

        [Required]
        public int MaxOccupancy { get; set; }

        public required string Description { get; set; }

        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("Hotel")]
        public Guid HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }
}
