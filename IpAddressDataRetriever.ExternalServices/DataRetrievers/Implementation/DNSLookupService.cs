using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using IpAddressDataRetriever.Services.Models.POCO;
using IpAddressDataRetriever.Services.Validators;
using IpAddressDataRetriever.Services.Values;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class DNSLookupService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://www.whoisxmlapi.com/whoisserver/DNSService?apiKey=at_Eda7RqS8lddc7sXGZS0nirsrZ6ohX&type=1&outputFormat=json&domainName=";

        public override async Task<JObject> RetrieveDataAsync(string domainName, int inputType)
        {
            JObject retrievedData = new JObject();

            if (!string.IsNullOrWhiteSpace(domainName))
            {
                //DNSLookups are only enabeled for Domains not IP Addresses
                if (inputType == InputTypes.DomainName)
                {
                    // Asynchronously get the JSON response.
                    ResponseData result = await ApiRetrieverAsync(endpointUrl + domainName);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(result.ResponseBody))
                    {
                        retrievedData.Add("dnsLookup", JObject.Parse(result.ResponseBody).GetValue("DNSData"));

                    }
                    else
                    {
                        retrievedData.Add("dnsLookup", "Service not available");
                    }
                }
                else
                {
                    if (inputType == InputTypes.IpAddressv4 || inputType == InputTypes.IpAddressv6)
                    {
                        retrievedData.Add("dnsLookup", "IpOrDomain param is an IP Address, DNS Lookup is unavailable for such value");
                    }
                }

            }


            return retrievedData;
        }

    }
}
