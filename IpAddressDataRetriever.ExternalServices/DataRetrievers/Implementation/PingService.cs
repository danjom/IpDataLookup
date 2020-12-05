using IpAddressDataRetriever.Services.DataRetrivers.Abstraction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Implementation
{
    public class PingService : IDataRetriever
    {
        private const int PingAttempts = 5;
        private const int DefaultPort = 80;

        public async Task<JObject> RetrieveDataAsync(string domainOrIp, int inputType)
        {
            JObject retrievedData = new JObject();

            try
            {

                bool onDebug = false;

#if DEBUG
                onDebug = true;

                Ping pingSender = new Ping();

                string[] pingResponses = new string[PingAttempts];

                for (int i = 0; i < PingAttempts; i++)
                {
                    PingReply reply = await pingSender.SendPingAsync(domainOrIp);

                    if (reply != null)
                    {
                        pingResponses[i] = $"{reply.Buffer.Length} bytes from {reply.Address}:" +
                                      $" icmp_seq={i} status={reply.Status} time={reply.RoundtripTime}ms";
                    }
                    else
                    {
                        pingResponses[i] = $"{reply.Address} TIMEOUT";
                    }
                }

                //retrievedData.Add("Ping Results", JArray.Parse(JsonConvert.SerializeObject(pingResponses)));

#endif
                
                if (!onDebug)
                {
                    var client = new TcpClient();
                    var result = client.BeginConnect(domainOrIp, DefaultPort, null, null);

                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));

                    if(success)
                        retrievedData.Add("Ping Results", domainOrIp + " is Up and Running -- Limited Data because Azure don't allow ICMP packets to avoid DoS Attacks");
                    else
                        retrievedData.Add("Ping Results", domainOrIp + ": Unable to establish connection using TCP Client");

                }



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
