using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class GeoIpService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://ip-geolocation.whoisxmlapi.com/api/v1?apiKey=at_Eda7RqS8lddc7sXGZS0nirsrZ6ohX&";

        public override async Task<JObject> RetrieveDataAsync(string domainName)
        {
            JObject retrievedData = new JObject();

            // Asynchronously get the JSON response.
            string result = await ApiRetrieverAsync(endpointUrl + domainName);

            if (!string.IsNullOrWhiteSpace(result))
            {
                retrievedData.Add("Geo Data", JObject.Parse(result));

            }
            else
            {
                retrievedData.Add("Geo Data", "Service not available");
            }

            return retrievedData;
        }

    }
}
