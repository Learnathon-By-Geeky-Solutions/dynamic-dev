using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.ViewModels
{
    public class BusSeatsViewModel
    {

        public Bus? Bus { get; set; }
        public List<Seat>? Seats { get; set; }
    }
}
