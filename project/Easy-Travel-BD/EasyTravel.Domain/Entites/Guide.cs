using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class Guide : IEntity<Guid>
    {
        public Guid Id { get; set; }

        
        [StringLength(100)]
        public required string FirstName { get; set; }

        
        [StringLength(100)]
        public required string LastName { get; set; }

        
        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public required string ContactNumber { get; set; }

        public required string Address { get; set; }

        public required string ProfilePicture { get; set; }

        [MaxLength(500)]
        public required string Bio { get; set; }

        [DataType(DataType.Date)]
        public required DateTime DateOfBirth { get; set; }

        [StringLength(500)]

        public required string LanguagesSpoken { get; set; }

        public required string Specialization { get; set; }

        public required int YearsOfExperience { get; set; }

        public required string LicenseNumber { get; set; }

        public required bool Availability { get; set; }

        public required decimal HourlyRate { get; set; }

        public decimal Rating { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime UpdatedAt { get; set; }


        public string? Status { get; set; }

        public required Guid AgencyId { get; set; } // Foreign Key

        public Agency? Agency { get; set; } // Navigation Property

    }

}
