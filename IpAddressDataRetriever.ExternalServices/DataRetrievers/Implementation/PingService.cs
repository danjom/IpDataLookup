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
        private const int PingAttempts = 4;
        private const int DefaultPort = 80;

        public async Task<JObject> RetrieveDataAsync(string domainOrIp, int inputType)
        {
            JObject retrievedData = new JObject();

            if (!string.IsNullOrWhiteSpace(domainOrIp))
            {

                try
                {

                    bool onDebug = false;

#if DEBUG
                    onDebug = true;

                    Ping pingSender = new Ping();

                    string[] pingResponses = new string[PingAttempts];

                    for (int i = 0; i < PingAttempts; i++)
                    {
                        PingReply reply = await pingSender.SendPingAsync(domainOrIp, 3500);

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

                    retrievedData.Add("Ping Results", JArray.Parse(JsonConvert.SerializeObject(pingResponses)));

#endif

                    if (!onDebug)
                    {
                        using (var tcpClient = new TcpClient())
                        {
                            var cancelTask = Task.Delay(new TimeSpan(0,0,5));
                            var connectTask = tcpClient.ConnectAsync(domainOrIp, DefaultPort);

                            //double await so if cancelTask throws exception, this throws it
                            await await Task.WhenAny(connectTask, cancelTask);

                            if (cancelTask.IsCompleted)
                            {
                                //If cancelTask and connectTask both finish at the same time,
                                //we'll consider it to be a timeout. 
                                throw new Exception("Timed out");
                            }

                            var success = tcpClient.Connected;

                            if (success)
                                retrievedData.Add("Ping Results", domainOrIp + " is Up and Running -- Limited Data because Azure don't allow ICMP packets to avoid DoS Attacks");
                            else
                                retrievedData.Add("Ping Results", domainOrIp + ": Unable to establish connection using TCP Client");
                        };

                    }



                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught!!!");
                    Console.WriteLine("Source : " + e.Source);
                    Console.WriteLine("Message : " + e.Message);

                    retrievedData.Add("Ping Results", "Unable to perform ping");
                }
            }

            return retrievedData;
        }
    }
}
