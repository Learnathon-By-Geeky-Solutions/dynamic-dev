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

        
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ProfilePicture { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public DateTime HireDate { get; set; }

        
        public string Status { get; set; }

        public string SocialMediaLinks { get; set; }

        public string Skills { get; set; }

        public decimal Rating { get; set; }

        public string PortfolioUrl { get; set; }

        public int AgencyId { get; set; } // Foreign Key
        public Agency Agency { get; set; } // Navigation 
    }
}
