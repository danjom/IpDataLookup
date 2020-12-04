using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using IpAddressDataRetriever.Services.Models.POCO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class RdapService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://rdap.verisign.com/com/v1/domain/";

        public override async Task<JObject> RetrieveDataAsync(string domainName)
        {

            JObject retrievedData = new JObject();

            // Asynchronously get the JSON response.
            ResponseData result = await ApiRetrieverAsync(endpointUrl + domainName);

            if (result.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(result.ResponseBody))
            {
                retrievedData.Add("RDAP", JObject.Parse(result.ResponseBody));

            }
            else
            {
                retrievedData.Add("RDAP", "Service not available");
            }

            return retrievedData;
        }

    }
}
