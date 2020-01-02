using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SocialChef.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            logger.LogInformation("Got weather.");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = summaries[rng.Next(summaries.Length)]
                })
                .ToArray();
        }
    }
}