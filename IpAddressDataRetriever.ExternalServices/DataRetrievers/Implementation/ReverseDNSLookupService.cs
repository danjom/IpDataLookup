using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using IpAddressDataRetriever.Services.Models.POCO;
using IpAddressDataRetriever.Services.Validators;
using IpAddressDataRetriever.Services.Values;
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

            int inputType = DataValidator.GetInputType(ipAddress);

            //DNSLookups are only enabeled for Domains not IP Addresses
            if (inputType == InputTypes.IpAddressv4 || inputType == InputTypes.IpAddressv6)
            {
                // Asynchronously get the JSON response.
                ResponseData result = await ApiRetrieverAsync(endpointUrl + ipAddress);

                if (result.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(result.ResponseBody))
                {
                    retrievedData.Add("Reverse DNS Lookup", JArray.Parse(JsonConvert.SerializeObject(JObject.Parse(result.ResponseBody).GetValue("result"))));

                }
                else
                {
                    retrievedData.Add("Reverse DNS Lookup", "Service not available");
                }
            }
            else
            {
                if (inputType == InputTypes.DomainName)
                {
                    retrievedData.Add("Rever DNS Lookup", "IpOrDomain param is a Domain Name, Reverse DNS Lookup is unavailable for such value");
                }
            }
                

            return retrievedData;
        }
    }
}
