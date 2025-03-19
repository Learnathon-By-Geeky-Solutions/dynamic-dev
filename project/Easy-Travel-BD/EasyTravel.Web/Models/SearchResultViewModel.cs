using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Models
{
    public class SearchResultViewModel
    {
        public SearchFormModel? Model { get; set; }
        public string? EventType { get; set; }
        public string? EventLocation { get; set; }
        public decimal TotalAmount { get; set; }
        public Photographer? Photographer { get; set; }
        public IEnumerable<Photographer>? Photographers { get; set; }
    }
}
