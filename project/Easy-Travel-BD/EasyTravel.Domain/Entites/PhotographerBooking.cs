using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Entites
{
    public class PhotographerBooking : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public int TotalTime { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PhotographerId { get; set; }
        public Photographer Photographer { get; set; }
    }
}
