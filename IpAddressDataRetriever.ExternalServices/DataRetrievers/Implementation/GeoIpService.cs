using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using IpAddressDataRetriever.Services.Models.POCO;
using IpAddressDataRetriever.Services.Validators;
using IpAddressDataRetriever.Services.Values;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class GeoIpService : APIDataRetriever
    {
        //Api Key must me securely stored, for example using a KeyVault
        private const string endpointUrl = "https://ip-geolocation.whoisxmlapi.com/api/v1?apiKey=at_Eda7RqS8lddc7sXGZS0nirsrZ6ohX&";

        public override async Task<JObject> RetrieveDataAsync(string ipAddrOrDomainName, int inputType)
        {
            JObject retrievedData = new JObject();

            if (!string.IsNullOrWhiteSpace(ipAddrOrDomainName)) 
            {
                string param = DataValidator.GetInputType(ipAddrOrDomainName) switch
                {
                    InputTypes.IpAddressv4 or InputTypes.IpAddressv6 => "ipAddress=" + ipAddrOrDomainName,
                    InputTypes.DomainName => "domain=" + ipAddrOrDomainName,
                    _ => throw new InvalidOperationException()
                };


                // Asynchronously get the JSON response.
                ResponseData result = await ApiRetrieverAsync(endpointUrl + param);

                if (result.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(result.ResponseBody))
                {
                    retrievedData.Add("geoIp", JObject.Parse(result.ResponseBody));

                }
                else
                {
                    retrievedData.Add("geoIp", "Unable to retrieve geodata");
                }
            }
            

            return retrievedData;
        }

    }
}
