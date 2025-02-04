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
        public string FirstName { get; set; }

        
        [StringLength(100)]
        public string LastName { get; set; }

        
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ProfilePicture { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        [StringLength(500)]
        public string LanguagesSpoken { get; set; }

        public string Specialization { get; set; }

        public int YearsOfExperience { get; set; }

        public string Certifications { get; set; }

        public string LicenseNumber { get; set; }

        public bool Availability { get; set; }

        public decimal HourlyRate { get; set; }

        public decimal Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        
        public string Status { get; set; }

        public int AgencyId { get; set; } // Foreign Key

        public Agency Agency { get; set; } // Navigation Property

    }

}
