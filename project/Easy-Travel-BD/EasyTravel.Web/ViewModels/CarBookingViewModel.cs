using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.ViewModels
{
    public class CarBookingViewModel
    {

        public Guid CarId { get; set; }
        public Car? Car { get; set; } 
        public BookingForm? BookingForm { get; set; } 
    }
}
