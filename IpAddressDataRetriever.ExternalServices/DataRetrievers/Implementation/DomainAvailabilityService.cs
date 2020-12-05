using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using IpAddressDataRetriever.Services.Models.POCO;
using IpAddressDataRetriever.Services.Validators;
using IpAddressDataRetriever.Services.Values;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class DomainAvailabilityService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://domain-availability.whoisxmlapi.com/api/v1?apiKey=at_Eda7RqS8lddc7sXGZS0nirsrZ6ohX&credits=DA&domainName=";

        public override async Task<JObject> RetrieveDataAsync(string domainName, int inputType)
        {
            JObject retrievedData = new JObject();

            //RDAPs are only enabeled for Domains not IP Addresses
            if (inputType == InputTypes.DomainName)
            {
                // Asynchronously get the JSON response.
                ResponseData result = await ApiRetrieverAsync(endpointUrl + domainName);

                if (result.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(result.ResponseBody))
                {
                    retrievedData.Add("Domain Availability", JObject.Parse(result.ResponseBody).GetValue("DomainInfo"));

                }
                else
                {
                    retrievedData.Add("Domain Availability", "Unable to retrieve availability data");
                }
            }
            else
            {
                if (inputType == InputTypes.IpAddressv4 || inputType == InputTypes.IpAddressv6)
                {
                    retrievedData.Add("Domain Availability", "IpOrDomain param is an IP Address, Domain Availability is unavailable for such value");
                }
            }

                

            return retrievedData;
        }

    }
}
