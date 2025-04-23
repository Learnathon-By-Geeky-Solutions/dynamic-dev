using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace EasyTravel.Web.Controllers
{       [Route("api/[controller]")]
        [ApiController]
    public class RecommendationController : Controller
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }
        [HttpGet("{type}")]
        public async Task<IActionResult> Get(string type, int count = 5)
        {
            var result = await _recommendationService.GetRecommendationsAsync(type, count);
            return Ok(result);
        }

    }
}
