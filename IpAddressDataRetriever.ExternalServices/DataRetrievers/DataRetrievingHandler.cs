using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using IpAddressDataRetriever.Services.DataRetrivers.Implementation;
using IpAddressDataRetriever.Services.Values;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrievers
{
    public class DataRetrievingHandler
    {
        public async Task<JObject> RetrieveData(string param, string service)
        {
            JObject result = null;


            try
            {
                IDataRetriever dataInspector;

                dataInspector = service switch
                {
                    DataServices.DNSLookup => new DNSLookupService(),
                    DataServices.ReverseDNSLookup => new ReverseDNSLookupService(),
                    DataServices.DomainAvailability => new DomainAvailabilityService(),
                    DataServices.GeoIp => new GeoIpService(),
                    DataServices.IpAddress => new IpAddressService(),
                    DataServices.Ping => new PingService(),
                    DataServices.WhoIs => new WhoIsService(),
                    DataServices.RDAP => new RdapService(),
                    _ => null,
                };

                if(dataInspector != null)
                {
                    result = await dataInspector.RetrieveDataAsync(param);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

            return result;
        }
    }
}
