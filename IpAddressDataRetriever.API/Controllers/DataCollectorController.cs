using IpAddressDataRetriever.API.Handlers;
using IpAddressDataRetriever.API.Models.POCO;
using IpAddressDataRetriever.Services.DataRetrievers;
using IpAddressDataRetriever.Services.DataRetrivers;
using IpAddressDataRetriever.Services.DataRetrivers.Implementation;
using IpAddressDataRetriever.Services.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
    public class DataCollectorController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<DataCollectorController> _logger;

        public DataCollectorController(ILogger<DataCollectorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            string hostName = "swimlane.com";
            string ipAddress = "31.13.67.35";

            string[] services = { "DomainAvailability", "GeoIp", "IpAddress", "WhoIs", "Ping", "DNSLookup" };

            List<RequestChunk> requestChuncks = LoadBalancer.SplitServices(services.ToList(), 3, true);


            DataRetrieverOrchestrator dataRetrieverOrchestrator = new DataRetrieverOrchestrator();
            JObject response = await dataRetrieverOrchestrator.OrquestrateRetrieval(services.ToList(), hostName);


            return Ok(response);
        }
    }
}
