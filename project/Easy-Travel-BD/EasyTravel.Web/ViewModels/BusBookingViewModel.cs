using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;

namespace EasyTravel.Web.ViewModels
{
    public class BusBookingViewModel
    {
        public Bus Bus { get; set; }
        public BookingForm BookingForm { get; set; }
        public List<string> SelectedSeatNumbers { get; set; } = new List<string>();
        public decimal TotalAmount { get; set; }
    }
}
