using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class Photographer : IEntity<Guid>
    {
        public Guid Id { get; set; }

        
        public required string FirstName { get; set; }

        
        public required string LastName { get; set; }

        
        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public required string PhoneNumber { get; set; }

        public required string Address { get; set; }

        public required string ProfilePicture { get; set; }

        [MaxLength(500)]
        public required string Bio { get; set; }

        [DataType(DataType.Date)]
        public required DateTime DateOfBirth { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; } = "Active";

        public string? SocialMediaLinks { get; set; }

        public required string Skills { get; set; }

        public decimal Rating { get; set; }

        public string? PortfolioUrl { get; set; }

        public required Guid AgencyId { get; set; } // Foreign Key
        public Agency Agency { get; set; } // Navigation 
    }
}
