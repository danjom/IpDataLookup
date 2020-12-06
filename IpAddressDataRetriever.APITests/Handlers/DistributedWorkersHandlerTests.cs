using Microsoft.VisualStudio.TestTools.UnitTesting;
using IpAddressDataRetriever.API.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IpAddressDataRetriever.API.Models.POCO;
using Newtonsoft.Json.Linq;
using IpAddressDataRetriever.Services.Values;

namespace IpAddressDataRetriever.API.Handlers.Tests
{
    [TestClass()]
    public class DistributedWorkersHandlerTests
    {
        [TestMethod()]
        public void RetrieveDataFromWorkerAsyncTest()
        {
            int workersCount;
            bool masterIncluded;
            string param;
            int inputType;
            List<RequestChunk> input;
            JObject response;

            //CASE equal workers than services
            param = "google.com";
            inputType = InputTypes.DomainName;
            workersCount = 2;
            masterIncluded = false;
            string[] services1 = { "geoip", "domainavailability" };

            input = LoadBalancer.SplitServices(services1.ToList(), workersCount, masterIncluded);

            response = DistributedWorkersHandler.RetrieveDataFromWorkerAsync(input, param, workersCount, inputType).Result;

            Assert.IsNotNull(response);

            //CASE more services than workers
            param = "facebook.com";
            inputType = InputTypes.DomainName;
            workersCount = 3;
            masterIncluded = true;
            string[] services2 = { "dnslookup", "reversednslookup", "domainavailability", "geoip", "ipaddress", "ping", "whois", "rdap" };

            input = LoadBalancer.SplitServices(services2.ToList(), workersCount, masterIncluded);

            response = DistributedWorkersHandler.RetrieveDataFromWorkerAsync(input.GetRange(0, workersCount - 1), param, workersCount - 1, inputType).Result;

            Assert.IsNotNull(response);

            //CASE more workers than services
            param = "yahoo.com";
            inputType = InputTypes.DomainName;
            workersCount = 5;
            masterIncluded = true;
            string[] services3 = { "dnslookup", "reversednslookup", "domainavailability" };

            input = LoadBalancer.SplitServices(services3.ToList(), workersCount, masterIncluded);

            response = DistributedWorkersHandler.RetrieveDataFromWorkerAsync(input, param, input?.Count ?? 0, inputType).Result;

            Assert.IsNotNull(response);

            //CASE no workers
            param = "yahoo.com";
            inputType = InputTypes.DomainName;
            workersCount = 0;
            masterIncluded = false;
            string[] services4 = { "dnslookup", "reversednslookup", "domainavailability" };

            input = LoadBalancer.SplitServices(services4.ToList(), workersCount, masterIncluded);

            response = DistributedWorkersHandler.RetrieveDataFromWorkerAsync(input, param, 0, inputType).Result;

            Assert.IsNotNull(response);

            //CASE no services
            param = "mcdonalds.com";
            inputType = InputTypes.DomainName;
            workersCount = 3;
            masterIncluded = false;
            string[] services5 = { };

            input = LoadBalancer.SplitServices(services5.ToList(), workersCount, masterIncluded);

            response = DistributedWorkersHandler.RetrieveDataFromWorkerAsync(input, param, workersCount, inputType).Result;

            Assert.IsNotNull(response);
        }
    }
}