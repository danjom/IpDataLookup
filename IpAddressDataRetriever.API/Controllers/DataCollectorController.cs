using IpAddressDataRetriever.API.Handlers;
using IpAddressDataRetriever.API.Models.POCO;
using IpAddressDataRetriever.Services.DataRetrievers;
using IpAddressDataRetriever.Services.Validators;
using IpAddressDataRetriever.Services.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
        private static readonly string[] AvailableServices = new[]
        {
            "DNSLookup", "ReverseDNSLookup", "DomainAvailability", "GeoIp", "IpAddress", "Ping", "WhoIs", "RDAP"
        };

        private readonly ILogger<DataCollectorController> _logger;

        public DataCollectorController(ILogger<DataCollectorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery(Name = "ipOrDomain")]  string ipOrDomain, [FromQuery(Name = "services")] string[] services)
        {
            //string hostName = "swimlane.com";
            //string ipAddress = "31.13.67.35";

            //string[] services = { "RDAP", "GeoIp", "IpAddress", "WhoIs", "Ping", "DNSLookup" };


            //Needs to determine whether is an Ip or a domain
            int inputType = DataValidator.GetInputType(ipOrDomain);
            
            if(inputType != InputTypes.Invalid)
            {
                //In case no services go in the query, all services are set
                if (services?.Length == 0)
                {
                    services = AvailableServices;
                }

                List<RequestChunk> requestChuncks = LoadBalancer.SplitServices(services.ToList(), 3, true);

                JObject response = await DataRetrieverOrchestrator.OrquestrateRetrieval(services.ToList(), ipOrDomain);

                return Ok(response);
            }
            else
            {
                return BadRequest(new BasicResponse { MsgContent = "Invalid ipOrDomain param, please make sure to set a valid ip address or domain name" });
            }

        }
    }
}
