using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTravel.Domain.Models
{
    public class Bus
    {
        [Key]
        public Guid BusServiceId { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Bus Service Name")]
        public required string BusServiceName { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("From")]
        public required string From { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("To")]
        public required string To { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Rate must be a positive value.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Rate")]
        public required decimal Rate { get; set; }

        [Required]
        [MaxLength(300)]
        [DisplayName("Service Details")]
        public required string ServiceDetails { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Stoppages")]
        public required string Stoppages { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Bus Contact")]
        public required string BusContact { get; set; }
    }
}
