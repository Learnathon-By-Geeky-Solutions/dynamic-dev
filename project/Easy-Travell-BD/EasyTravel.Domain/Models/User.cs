using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Models
{
   
        public class User
        {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Name")]
        public required string Name { get; set; }
        [Required]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email")]
        public required string Email { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Password")]
        public required string Password { get; set; }
        [Required]
        [MaxLength(50)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [DisplayName("Phone Number")]
        public required string PhoneNumber { get; set; }
        [MaxLength(50)]
        [DisplayName("Profession")]
        public string? Profession { get; set; }
        [Required]
        [DisplayName("Role")]
        public required string Role { get; set; } 
        }
    
}
