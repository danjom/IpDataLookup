using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrievers
{
    public static class DataRetrieverOrchestrator
    {
        public static async Task<JObject> OrquestrateRetrieval(List<string> serviceNames, string ipOrDomain)
        {
            JObject requestedData = new JObject();

            try
            {
                DataRetrievingHandler retrievingHandler = new DataRetrievingHandler();

                List<Task<JObject>> retrievalTasks = new List<Task<JObject>>();
                Task<JObject> currentTask = null;

                //Create the tasks
                for(int i = 0; i < serviceNames?.Count; ++i)
                {
                    currentTask = retrievingHandler.RetrieveData(ipOrDomain, serviceNames[i]);

                    retrievalTasks.Add(currentTask);
                }

                //Now it's time to merge the response from each task to have a single json response
                while (retrievalTasks.Any())
                {
                    Task<JObject> finishedTask = await Task.WhenAny(retrievalTasks);
                    retrievalTasks.Remove(finishedTask);
                    requestedData.Merge(await finishedTask, new JsonMergeSettings
                    {
                        // Union array values together to avoid duplicates
                        MergeArrayHandling = MergeArrayHandling.Concat
                    });
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

            return requestedData;
        }
    }
}
