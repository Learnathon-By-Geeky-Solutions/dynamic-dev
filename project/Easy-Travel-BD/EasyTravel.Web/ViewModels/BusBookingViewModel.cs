using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;

namespace EasyTravel.Web.ViewModels
{
    public class BusBookingViewModel
    {

        public Guid BusId { get; set; }
        public Bus? Bus { get; set; }

        public BookingForm? BookingForm { get; set; }
        public List<string>? SelectedSeatNumbers { get; set; } 
        public List<Guid>? SelectedSeatIds { get; set; } 
        public decimal TotalAmount { get; set; }
    }
}
