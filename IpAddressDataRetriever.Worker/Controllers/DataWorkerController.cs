using IpAddressDataRetriever.Services.DataRetrievers;
using IpAddressDataRetriever.Services.Values;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Worker.Controllers
{
    [RequireHttps]
    [ApiController]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DataWorkerController : ControllerBase
    {
        //This is just for demo purposes, for a real product, the API Key generation and validation needs to be implemented
        private static readonly string ApiKey = "xevUntljUUqoeankdKmnYFFqEXTGYEpi";
        //qwbNVcIlkjMDBhZuUMUYVIrPUpcVd6IJ //This is the other worker key

        private readonly ILogger<DataWorkerController> _logger;

        public DataWorkerController(ILogger<DataWorkerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery(Name = "ipOrDomain")] string ipOrDomain, [FromQuery(Name = "services")] string[] services, [FromQuery(Name = "inputType")] int inputType, [FromQuery(Name = "apiKey")] string apiKey)
        {

            IActionResult result;

            //Since this is a microservice I'm asumming that @inputType and @services arrive with valid values
            if (!string.IsNullOrWhiteSpace(ipOrDomain) && string.Compare(apiKey, ApiKey) == 0 && inputType != InputTypes.Invalid && services?.Length > 0 )
            {
                JObject response = await DataRetrieverOrchestrator.OrquestrateRetrieval(services.ToList(), ipOrDomain);

                result = Ok(response);
            }
            else
            {
                result = BadRequest();
            }

            return result;
        }
    }
}
