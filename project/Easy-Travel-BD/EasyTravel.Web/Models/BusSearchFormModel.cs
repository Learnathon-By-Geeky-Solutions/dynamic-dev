namespace EasyTravel.Web.Models
{
    public class BusSearchFormModel
    {
     
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }

    }
}
