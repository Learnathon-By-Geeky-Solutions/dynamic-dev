using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Models
{
    public class BookingModel
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public PhotographerBooking? PhotographerBooking { get; set; }
        public GuideBooking? GuideBooking { get; set; }
        public CarBooking? CarBooking { get; set; }
        public BusBooking? BusBooking { get; set; }
        public HotelBooking? HotelBooking { get; set; }
        public Booking? Booking { get; set; }
    }
}
