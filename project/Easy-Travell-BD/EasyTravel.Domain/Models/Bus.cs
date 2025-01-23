using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Models
{
    public class Bus
    {

        [Key]
        public Guid BusServiceId { get; set; }
        public required string BusServiceName { get; set; }
        public required string From { get; set; }
        public required string To { get; set; }
        public required decimal Rate { get; set; }
        public required string ServicesDetails { get; set; }
        public required string Stoppages { get; set; }
        public required string BusContact { get; set; }
    }
}