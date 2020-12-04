﻿using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using IpAddressDataRetriever.Services.Models.POCO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class DomainAvailabilityService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://domain-availability.whoisxmlapi.com/api/v1?apiKey=at_Eda7RqS8lddc7sXGZS0nirsrZ6ohX&credits=DA&domainName=";

        public override async Task<JObject> RetrieveDataAsync(string domainName)
        {
            JObject retrievedData = new JObject();

            // Asynchronously get the JSON response.
            ResponseData result = await ApiRetrieverAsync(endpointUrl + domainName);

            if (result.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(result.ResponseBody))
            {
                retrievedData.Add("Domain Availability", JObject.Parse(result.ResponseBody).GetValue("DomainInfo"));

            }
            else
            {
                retrievedData.Add("Domain Availability", "Service not available");
            }

            return retrievedData;
        }

    }
}
