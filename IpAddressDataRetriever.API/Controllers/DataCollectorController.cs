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

        private static int WorkersCount = 2;//Will be 2 remote workers
        private static readonly string[] AvailableServices = new[]
        {
            "dnslookup", "reversednslookup", "domainavailability", "geoip", "ipaddress", "ping", "whois", "rdap"
        };

        private readonly ILogger<DataCollectorController> _logger;

        public DataCollectorController(ILogger<DataCollectorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery(Name = "ipOrDomain")]  string ipOrDomain, [FromQuery(Name = "services")] string[] services, [FromQuery(Name = "useMasterAsWorker")] bool useMasterAsWorker)
        
        {
            IActionResult result = null;

            bool valid = true;

            if (!string.IsNullOrWhiteSpace(ipOrDomain))
            {
                //Needs to determine whether is an Ip or a domain
                int inputType = DataValidator.GetInputType(ipOrDomain);

                if (inputType != InputTypes.Invalid)
                {
                    //In case of repeated, deletes them
                    services = services.Distinct().Select(s => s.ToLowerInvariant()).ToArray();

                    //In case no services go in the query, all services are set
                    if (services?.Length == 0)
                    {
                        services = AvailableServices;
                    }


                    //Now will split data for sending it to workers
                    List<RequestChunk> requestChuncks;
                    List<Task<JObject>> workerTasks = new List<Task<JObject>>();

                    //Now will define the chunks, if master will be used then workers are incresed by 1 to make the calculations
                    requestChuncks = LoadBalancer.SplitServices(services.ToList(), useMasterAsWorker ? WorkersCount +1 : WorkersCount, useMasterAsWorker);

                    //If worked was splitted successfully, it's time to process each chunk of services
                    if (requestChuncks?.Count > 0)
                    {
                        //Since the master will also work, needs to remove its chuck before sending to the workers
                        if (useMasterAsWorker)
                        {
                            //Now will create the own task to process its own chunk
                            workerTasks.Add(DataRetrieverOrchestrator.OrquestrateRetrieval(requestChuncks.ElementAt(requestChuncks.Count - 1).Services.ToList(), ipOrDomain, inputType));

                            requestChuncks.RemoveAt(requestChuncks.Count - 1);
                        }


                        //Now creates the task which manage the workers
                        workerTasks.Add(DistributedWorkersHandler.RetrieveDataFromWorkerAsync(requestChuncks, ipOrDomain, WorkersCount, inputType));

                        JObject response = new JObject();

                        //Now it's time to merge the response from each task to have a single json response
                        while (workerTasks.Any())
                        {
                            Task<JObject> finishedTask = await Task.WhenAny(workerTasks);
                            workerTasks.Remove(finishedTask);
                            response.Merge(await finishedTask, new JsonMergeSettings
                            {
                                // Union array values together to avoid duplicates
                                MergeArrayHandling = MergeArrayHandling.Concat
                            });
                        }

                        result = Ok(response);
                    }
                    else
                    {
                        result = new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError);
                    }

                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }

            if (!valid)
            {
                result = BadRequest(new BasicResponse { MsgContent = "Invalid ipOrDomain param, please make sure to set a valid ip address or domain name" });
            }


            return result;

        }
    }
}
