using IpAddressDataRetriever.API.Models.POCO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.API.Handlers
{
    public static class DistributedWorkersHandler
    {
        public static readonly string[] externalWorkersUris = { "https://serviceworker1.azurewebsites.net/api/v1/dataworker?apiKey=xevUntljUUqoeankdKmnYFFqEXTGYEpi", "https://serviceworker2.azurewebsites.net/api/v1/dataworker?apiKey=qwbNVcIlkjMDBhZuUMUYVIrPUpcVd6IJ" };

        public static async Task<JObject> RetrieveDataFromWorkerAsync(List<RequestChunk> workingChunks, string ipOrDomain, int workersCount, int inputType)
        {
            JObject retrievedData = new JObject();

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response;
                List<Task<HttpResponseMessage>> httpResponseTasks = new List<Task<HttpResponseMessage>>();

                for(int i = 0; i < workingChunks?.Count; ++i)
                {
                    string uri = externalWorkersUris[i % workersCount] + "&inputType=" + inputType + "&ipOrDomain=" + ipOrDomain + "&services=" + string.Join("&services=", workingChunks[i].Services);

                    httpResponseTasks.Add(client.GetAsync(externalWorkersUris[i % workersCount] + "&inputType=" + inputType + "&ipOrDomain=" + ipOrDomain + "&services=" + string.Join("&services=", workingChunks[i].Services)));
                }

                //Now it's time to merge the response from each task to have a single json response
                while (httpResponseTasks.Any())
                {
                    Task<HttpResponseMessage> finishedTask = await Task.WhenAny(httpResponseTasks);
                    httpResponseTasks.Remove(finishedTask);

                    response = finishedTask.Result;

                    if(response != null)
                    {

                        retrievedData.Merge(JObject.Parse(response.Content.ReadAsStringAsync().Result), new JsonMergeSettings
                        {
                            // Union array values together to avoid duplicates
                            MergeArrayHandling = MergeArrayHandling.Concat
                        });
                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

            return retrievedData;
        }
    }
}
