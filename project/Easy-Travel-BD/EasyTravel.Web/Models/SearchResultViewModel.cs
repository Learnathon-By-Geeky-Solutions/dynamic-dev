using EasyTravel.Domain.Entites;

namespace EasyTravel.Web.Models
{
    public class SearchResultViewModel
    {
        public SearchFormModel? SearchFormModel { get; set; }
        public string? EventType { get; set; }
        public string? EventLocation { get; set; }
        public decimal TotalAmount { get; set; }
        public Photographer? Photographer { get; set; }
        public Guide? Guide { get; set; }
        public IEnumerable<Photographer>? Photographers { get; set; }
        public IEnumerable<Guide>? Guides { get; set; }
    }
}
