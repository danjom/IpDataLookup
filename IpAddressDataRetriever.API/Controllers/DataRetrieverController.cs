using IpAddressDataRetriever.Services.DataRetrivers;
using IpAddressDataRetriever.Services.DataRetrivers.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.API.Controllers
{
    [RequireHttps]
    [ApiController]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DataRetrieverController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<DataRetrieverController> _logger;

        public DataRetrieverController(ILogger<DataRetrieverController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            string hostName = "swimlane.com";
            string ipAddress = "31.13.67.35";

            IDataInspector inspectorService = new GeoIpService();

            inspectorService.RetrieveDataAsync("domain="+hostName);
            inspectorService.RetrieveDataAsync("ipAddress=" + ipAddress);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
