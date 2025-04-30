using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Views.Shared.Components.RecommendationCarousel
{
    public class RecommendationCarouselViewComponent : ViewComponent
    { 
        private readonly IRecommendationService _recommendationService;
        public RecommendationCarouselViewComponent(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string type, int count = 5)
        {
             var recommendations = await _recommendationService.GetRecommendationsAsync(type, count);
            return View("_CarouselPartial", recommendations);
        }
    }

}
