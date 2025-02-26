using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EasyTravel.Domain.Entites
{
    public class User : IdentityUser<Guid>
    {
        public override Guid Id {  get; set; }
        public required string? FirstName { get; set; }
        public required string? LastName { get; set; }
        public required string? Gender { get; set; }
        public required DateTime? DateOfBirth { get; set; }

        public DateTime? CreatedAt { get; set; }
        
    }
}
