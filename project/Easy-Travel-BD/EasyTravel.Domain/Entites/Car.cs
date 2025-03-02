using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class Car: IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string OperatorName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? CarType { get; set; }

        [Required]
        [MaxLength(50)]
        public required string From { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("To")]
        public required string To { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public required DateTime DepartureTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public required DateTime ArrivalTime { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }
      
        public string? ImagePath { get; set; }



    }
}
