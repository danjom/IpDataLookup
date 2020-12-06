using Microsoft.VisualStudio.TestTools.UnitTesting;
using IpAddressDataRetriever.Services.DataRetrivers.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using Newtonsoft.Json.Linq;
using IpAddressDataRetriever.Services.Values;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation.Tests
{
    [TestClass()]
    public class DomainAvailabilityServiceTests
    {
        [TestMethod()]
        public void RetrieveDataAsyncTest()
        {
            IDataRetriever dataRetriever = new DomainAvailabilityService();

            string inputString;
            int inputType;
            JObject response;

            //CASE DOMAIN NAME
            inputString = "facebook.com";
            inputType = InputTypes.DomainName;

            response = dataRetriever.RetrieveDataAsync(inputString, inputType).Result;

            Assert.IsNotNull(response);


            //CASE IP Address
            inputString = "45.232.119.146";
            inputType = InputTypes.IpAddressv4;

            response = dataRetriever.RetrieveDataAsync(inputString, inputType).Result;

            Assert.IsNotNull(response);

            //CASE Unmatched data
            inputString = "45.232.119.146";
            inputType = InputTypes.DomainName;

            response = dataRetriever.RetrieveDataAsync(inputString, inputType).Result;

            Assert.IsNotNull(response);

            //CASE Invalid
            inputString = "";
            inputType = InputTypes.Invalid;

            response = dataRetriever.RetrieveDataAsync(inputString, inputType).Result;

            Assert.IsNotNull(response);
        }
    }
}