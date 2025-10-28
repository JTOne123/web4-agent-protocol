using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TestWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

		[EndpointDescription("This method gets the weather forecast for a specific city")]
		[HttpGet(Name = "GetWeatherForecast")]
        public string Get([Description("City name value")] string cityName)
        {
            return $"Weather forecast for {cityName}: {Summaries[Random.Shared.Next(Summaries.Length)]}";
        }
    }
}
