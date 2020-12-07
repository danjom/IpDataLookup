using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using IpAddressDataRetriever.Services.Models.POCO;
using IpAddressDataRetriever.Services.Validators;
using IpAddressDataRetriever.Services.Values;
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

        public override async Task<JObject> RetrieveDataAsync(string domainName, int inputType)
        {

            JObject retrievedData = new JObject();

            if (!string.IsNullOrWhiteSpace(domainName))
            {

                //RDAPs are only enabeled for Domains not IP Addresses
                if (inputType == InputTypes.DomainName)
                {
                    // Asynchronously get the JSON response.
                    ResponseData result = await ApiRetrieverAsync(endpointUrl + domainName);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(result.ResponseBody))
                    {
                        retrievedData.Add("rdap", JObject.Parse(result.ResponseBody));

                    }
                    else
                    {
                        retrievedData.Add("rdap", "Unable to retrieve RDAP data");
                    }
                }
                else
                {
                    if (inputType == InputTypes.IpAddressv4 || inputType == InputTypes.IpAddressv6)
                    {
                        retrievedData.Add("rdap", "IpOrDomain param is an IP Address, RDAP is unavailable for such value");
                    }
                }
            }

            return retrievedData;
        }

    }
}
