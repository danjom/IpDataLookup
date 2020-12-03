using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class DNSLookupService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://www.whoisxmlapi.com/whoisserver/DNSService?apiKey=at_Eda7RqS8lddc7sXGZS0nirsrZ6ohX&type=1&outputFormat=json&domainName=";

        public override async Task<JObject> RetrieveDataAsync(string domainName)
        {
            JObject retrievedData = new JObject();

            // Asynchronously get the JSON response.
            string result = await ApiRetrieverAsync(endpointUrl + domainName);

            if (!string.IsNullOrWhiteSpace(result))
            {
                retrievedData.Add("DNS Lookup", JObject.Parse(result).GetValue("DNSData"));

            }
            else
            {
                retrievedData.Add("DNS Lookup", "Service not available");
            }

            return retrievedData;
        }

    }
}
