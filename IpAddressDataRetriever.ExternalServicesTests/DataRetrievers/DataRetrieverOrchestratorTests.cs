using Microsoft.VisualStudio.TestTools.UnitTesting;
using IpAddressDataRetriever.Services.DataRetrievers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using IpAddressDataRetriever.Services.Values;

namespace IpAddressDataRetriever.Services.DataRetrievers.Tests
{
    [TestClass()]
    public class DataRetrieverOrchestratorTests
    {
        [TestMethod()]
        public void OrquestrateRetrievalTest()
        {
            string param;
            int inputType;
            JObject response;

            //CASE 2 services
            param = "facebook.com";
            string[]  services1 = { "geoip", "domainavailability"};
            inputType = InputTypes.DomainName;

            response = DataRetrieverOrchestrator.OrquestrateRetrieval(services1.ToList(), param, inputType).Result;

            Assert.IsNotNull(response);


            //CASE all services
            param = "104.18.61.137";
            string[] services2 = { "dnslookup", "reversednslookup", "domainavailability", "geoip", "ipaddress", "ping", "whois", "rdap" };
            inputType = InputTypes.IpAddressv4;

            response = DataRetrieverOrchestrator.OrquestrateRetrieval(services2.ToList(), param, inputType).Result;

            Assert.IsNotNull(response);

            //CASE mixed valid and invalid services
            param = "45.232.119.146";
            string[] services3 = { "dnslookup", "revsednslookup", "domainilability", "geoip", "ipaddress", "ping", "whoi", "rdap" };
            inputType = InputTypes.IpAddressv4;

            response = DataRetrieverOrchestrator.OrquestrateRetrieval(services3.ToList(), param, inputType).Result;

            Assert.IsNotNull(response);

            //CASE all invalid services
            param = "45.232.119.146";
            string[] services4 = { "slookup", "revseslookup", "doinilability", "eoip" };
            inputType = InputTypes.IpAddressv4;

            response = DataRetrieverOrchestrator.OrquestrateRetrieval(services4.ToList(), param, inputType).Result;

            Assert.IsNotNull(response);

            //CASE no services
            param = "";
            string[] services5 = { };
            inputType = InputTypes.Invalid;

            response = DataRetrieverOrchestrator.OrquestrateRetrieval(services5.ToList(), param, inputType).Result;

            Assert.IsNotNull(response);
        }
    }
}