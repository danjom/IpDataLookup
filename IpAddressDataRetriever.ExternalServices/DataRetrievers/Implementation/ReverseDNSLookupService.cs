using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class ReverseDNSLookupService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://reverse-ip.whoisxmlapi.com/api/v1?apiKey=at_Eda7RqS8lddc7sXGZS0nirsrZ6ohX&outputFormat=json&ip=";

        public override async Task<JObject> RetrieveDataAsync(string ipAddress)
        {
            JObject retrievedData = new JObject();

            // Asynchronously get the JSON response.
            string result = await ApiRetrieverAsync(endpointUrl + ipAddress);

            if (!string.IsNullOrWhiteSpace(result))
            {
                retrievedData.Add("Reverse DNS Lookup", JArray.Parse(JsonConvert.SerializeObject(JObject.Parse(result).GetValue("result"))));

            }
            else
            {
                retrievedData.Add("Reverse DNS Lookup", "Service not available");
            }

            return retrievedData;
        }
    }
}
