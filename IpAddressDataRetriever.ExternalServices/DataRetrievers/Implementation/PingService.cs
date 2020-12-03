using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class PingService : IDataInspector
    {
        private const int PingAttempts = 5;

        public async Task<JObject> RetrieveDataAsync(string domainOrIp)
        {
            JObject retrievedData = new JObject();

            try
            {
                Ping pingSender = new Ping();

                string[] pingResponses = new string[PingAttempts];

                for (int i = 0; i < PingAttempts; i++)
                {
                    PingReply reply = await pingSender.SendPingAsync(domainOrIp);

                    if(reply != null)
                    {
                        pingResponses[i] = $"{reply.Buffer.Length} bytes from {reply.Address}:" +
                                      $" icmp_seq={i} status={reply.Status} time={reply.RoundtripTime}ms";
                    }
                    else
                    {
                        pingResponses[i] = $"{reply.Address} TIMEOUT";
                    }
                }

                retrievedData.Add("Ping Results", JArray.Parse(JsonConvert.SerializeObject(pingResponses)));
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);

                retrievedData.Add("Ping Results", "Service not available");
            }

            return retrievedData;
        }
    }
}
