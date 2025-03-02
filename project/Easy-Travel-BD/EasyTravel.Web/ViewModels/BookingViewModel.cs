using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.ViewModels
{
    public class BookingViewModel
    {
        public Bus Bus { get; set; }
        public BusBookingForm BookingForm { get; set; }
        public List<string> SelectedSeatNumbers { get; set; } = new List<string>();
    }
}
