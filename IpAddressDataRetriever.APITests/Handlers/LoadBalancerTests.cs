using Microsoft.VisualStudio.TestTools.UnitTesting;
using IpAddressDataRetriever.API.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IpAddressDataRetriever.Services.Values;
using IpAddressDataRetriever.API.Models.POCO;

namespace IpAddressDataRetriever.API.Handlers.Tests
{
    [TestClass()]
    public class LoadBalancerTests
    {
        [TestMethod()]
        public void SplitServicesTest()
        {
            int workersCount;
            bool masterIncluded;
            List<RequestChunk> response;

            //CASE equal workers than services
            workersCount = 2;
            masterIncluded = false;
            string[] services1 = { "geoip", "domainavailability" };

            response = LoadBalancer.SplitServices(services1.ToList(), workersCount, masterIncluded);

            Assert.IsNotNull(response);
            Assert.AreEqual(2, response?.Count);
            Assert.AreEqual(false, response?.ElementAt(1).MasterAssigned);


            //CASE more services than workers
            workersCount = 3;
            masterIncluded = true;
            string[] services2 = { "dnslookup", "reversednslookup", "domainavailability", "geoip", "ipaddress", "ping", "whois", "rdap" };

            response = LoadBalancer.SplitServices(services2.ToList(), workersCount, masterIncluded);

            Assert.IsNotNull(response);
            Assert.AreEqual(3, response?.Count);
            Assert.AreEqual(true, response?.ElementAt(2).MasterAssigned);

            //CASE more workers than services
            workersCount = 5;
            masterIncluded = true;
            string[] services3 = { "dnslookup", "reversednslookup", "domainavailability" };

            response = LoadBalancer.SplitServices(services3.ToList(), workersCount, masterIncluded);

            Assert.IsNotNull(response);
            Assert.AreEqual(3, response?.Count);
            Assert.AreEqual(false, response?.ElementAt(2).MasterAssigned);

            //CASE no workers
            workersCount = 0;
            masterIncluded = false;
            string[] services4 = { "dnslookup", "reversednslookup", "domainavailability" };

            response = LoadBalancer.SplitServices(services4.ToList(), workersCount, masterIncluded);

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response?.Count);

            //CASE no services
            workersCount = 3;
            masterIncluded = false;
            string[] services5 = { };

            response = LoadBalancer.SplitServices(services5.ToList(), workersCount, masterIncluded);

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response?.Count);
        }
    }
}