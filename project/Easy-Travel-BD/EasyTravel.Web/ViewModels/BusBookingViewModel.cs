using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.ViewModels
{
    public class BusBookingViewModel
    {
        public Bus Bus { get; set; }
        public BookingForm BookingForm { get; set; }
        public List<string> SelectedSeatNumbers { get; set; } = new List<string>();
        public List<Guid> SelectedSeatIds { get; set; } = new List<Guid>();
        public decimal TotalAmount { get; set; }
    }
}
