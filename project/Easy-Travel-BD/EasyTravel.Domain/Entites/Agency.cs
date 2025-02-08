using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class Agency : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        
        public required string Name { get; set; }
        
        public required string Address { get; set; }
        
        public required string ContactNumber { get; set; }
        
        public string? Website { get; set; }
        
        public required string LicenseNumber { get; set; }

        public DateTime AddDate { get; set; }

        //public List<Photographer>? Photographers { get; set; }
        //public List<Photographer>? Guides { get; set; }

    }
}
