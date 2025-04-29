using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Models
{
    public class SearchCarResultViewModel
    {
        public CarSearchFormModel? CarSearchFormModel { get; set; }
        public IEnumerable<Car>? Cars { get; set; }

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
