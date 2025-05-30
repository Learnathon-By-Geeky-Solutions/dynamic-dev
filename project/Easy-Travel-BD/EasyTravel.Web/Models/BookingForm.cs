﻿using System.ComponentModel.DataAnnotations;

namespace EasyTravel.Web.Models
{
    public class BookingForm
    {
        [Required(ErrorMessage = "Passenger name is required")]
        [MaxLength(100)]
        public string PassengerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}
