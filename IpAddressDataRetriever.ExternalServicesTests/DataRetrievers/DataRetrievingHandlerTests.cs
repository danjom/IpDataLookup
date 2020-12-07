using Microsoft.VisualStudio.TestTools.UnitTesting;
using IpAddressDataRetriever.Services.DataRetrievers.Handlers;
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
    public class DataRetrievingHandlerTests
    {
        [TestMethod()]
        public void RetrieveDataTest()
        {
            string param;
            string service;
            int inputType;
            JObject response;

            //CASE DOMAIN NAME
            param = "facebook.com";
            service = "geoip";
            inputType = InputTypes.DomainName;

            response = DataRetrievingHandler.RetrieveData(param, service, inputType).Result;

            Assert.IsNotNull(response);


            //CASE IP Address
            param = "104.18.61.137";
            service = "reversednslookup";
            inputType = InputTypes.IpAddressv4;

            response = DataRetrievingHandler.RetrieveData(param, service, inputType).Result;

            Assert.IsNotNull(response);

            //CASE Unmatched data
            param = "45.232.119.146";
            service = "dnslookup";
            inputType = InputTypes.DomainName;

            response = DataRetrievingHandler.RetrieveData(param, service, inputType).Result;

            Assert.IsNotNull(response);

            //CASE Invalid
            param = "";
            service = "-";
            inputType = InputTypes.Invalid;

            response = DataRetrievingHandler.RetrieveData(param, service, inputType).Result;

            Assert.IsNotNull(response);
        }
    }
}