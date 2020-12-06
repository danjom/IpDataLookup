using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class IpAddressService : IDataRetriever
    {
        public async Task<JObject> RetrieveDataAsync(string ipAddress, int inputType)
        {
            JObject retrievedData = new JObject();

            if (!string.IsNullOrWhiteSpace(ipAddress))
            {

                try
                {
                    IPAddress[] hostEntry = await Dns.GetHostAddressesAsync(ipAddress);

                    if (hostEntry?.Length > 0)
                    {
                        string[] ips = hostEntry.Select(ip => ip.ToString()).ToArray();

                        retrievedData.Add("IP Addresses", JArray.Parse(JsonConvert.SerializeObject(ips)));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught!!!");
                    Console.WriteLine("Source : " + e.Source);
                    Console.WriteLine("Message : " + e.Message);

                    retrievedData.Add("IP Addresses", "Unable to retrieve ip addresses data");
                }
            }

            return retrievedData;
        }
    }
}
