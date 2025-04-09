using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Models
{
    public class SearchCarResultViewModel
    {
        public BusSearchFormModel CarSearchFormModel { get; set; }
        public IEnumerable<Car> Cars { get; set; }


    }
}
