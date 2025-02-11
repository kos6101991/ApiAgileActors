using ApiAggileActors.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiAggileActors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregationController : ControllerBase
    {
        private readonly IOpenWeatherMapService _weatherService;
        private readonly INewsApiService _newsService;
        private readonly ITwitterService _twitterService;

        public AggregationController(IOpenWeatherMapService weatherService, INewsApiService newsService, ITwitterService twitterService)
        {
            _weatherService = weatherService;
            _newsService = newsService;
            _twitterService = twitterService;
        }


        [HttpGet("aggregate")]
        public async Task<IActionResult> AggregateData(string city, string query)
        {
            try
            {
                var weatherTask = _weatherService.GetWeatherAsync(city);
                var newsTask = _newsService.GetNewsAsync(query);
                var tweetsTask = _twitterService.GetTweetsAsync(query);

                await Task.WhenAll(weatherTask, newsTask, tweetsTask);

                var result = new
                {
                    Weather = await weatherTask,
                    News = await newsTask,
                    Tweets = await tweetsTask
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching data.", Details = ex.Message });
            }
        }
    }
}
