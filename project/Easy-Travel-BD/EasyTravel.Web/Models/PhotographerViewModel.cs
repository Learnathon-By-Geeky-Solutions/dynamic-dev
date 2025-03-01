using System.ComponentModel.DataAnnotations;

namespace EasyTravel.Web.Models
{
    public class PhotographerViewModel
    {

        [StringLength(100)]
        public string? FirstName { get; set; }


        [StringLength(100)]
        public  string? LastName { get; set; }


        [EmailAddress]
        public  string? Email { get; set; }

        [Phone]
        public  string? ContactNumber { get; set; }

        public  string? Address { get; set; }

        public  string? ProfilePicture { get; set; }

        [MaxLength(500)]
        public  string? Bio { get; set; }

        [DataType(DataType.Date)]
        public  DateTime DateOfBirth { get; set; }

        public  string? Skills { get; set; }
        public  string? PortfolioUrl { get; set; }

        public  string? Specialization { get; set; }

        public  int YearsOfExperience { get; set; }

        public  decimal HourlyRate { get; set; }

        public decimal Rating { get; set; }

        public string? SocialMediaLinks { get; set; }
    }
}
