using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class WhoIsService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://www.whoisxmlapi.com/whoisserver/WhoisService?apiKey=at_Eda7RqS8lddc7sXGZS0nirsrZ6ohX&outputFormat=json&domainName=";

        public override async Task<JObject> RetrieveDataAsync(string domainName)
        {

            JObject retrievedData = new JObject();

            // Asynchronously get the JSON response.
            string result = await ApiRetrieverAsync(endpointUrl + domainName);

            if (!string.IsNullOrWhiteSpace(result))
            {
                retrievedData.Add("WhoIs", JObject.Parse(result).GetValue("WhoisRecord"));

            }
            else
            {
                retrievedData.Add("WhoIs", "Service not available");
            }

            return retrievedData;
        }

    }
}
