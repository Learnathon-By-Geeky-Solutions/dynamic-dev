using System.ComponentModel.DataAnnotations;

namespace EasyTravel.Web.Models
{
    public class BookingFormViewModel
    {
        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public Guid PhotographerId { get; set; }
    }
}
