using IpAddressDataRetriever.Services.DataRetrivers;
using IpAddressDataRetriever.Services.DataRetrivers.Implementation;
using IpAddressDataRetriever.Services.Values;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    DataServices.DomainAvailability => new DomainAvailabilityService(),
                    DataServices.GeoIp => new GeoIpService(),
                    DataServices.IpAddress => new IpAddressService(),
                    DataServices.Ping => new PingService(),
                    DataServices.ReverseDNSLookup => new ReverseDNSLookupService(),
                    DataServices.WhoIs => new WhoIsService(),
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
