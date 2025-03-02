using System.ComponentModel.DataAnnotations;

namespace EasyTravel.Web.ViewModels
{
    public class BusBookingForm
    {
        [Required(ErrorMessage = "Passenger name is required")]
        [MaxLength(100)]
        public string PassengerName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
