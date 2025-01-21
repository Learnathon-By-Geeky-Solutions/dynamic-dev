using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Models
{
   
        public class User
        {
            [Key]
            public int UserId { get; set; }
            public required string Name { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required string PhoneNumber { get; set; }
            public string? Profession { get; set; }  
            public required string Role { get; set; } 
        }
    
}
