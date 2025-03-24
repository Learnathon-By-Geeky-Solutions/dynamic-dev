using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Models
{
    public class HotelBookingViewModel
    {
        public required Hotel? hotel { get; set; }
        public required Room? room { get; set; }
        public  HotelBooking ? hotelBooking { get; set; }
    }
}