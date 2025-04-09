using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Models
{
    public class SearchBusResultViewModel
    {

        public BusSearchFormModel busSearchFormModel { get; set; }
        public IEnumerable<Bus> Buses { get; set; }
    }
}
