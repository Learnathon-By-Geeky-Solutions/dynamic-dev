using EasyTravel.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace EasyTravel.Web.Models
{
    public class SearchFormModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Range(1, 24, ErrorMessage = "Hours must be between 1 and 24.")]
        public int TimeInHour { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

    }
}
