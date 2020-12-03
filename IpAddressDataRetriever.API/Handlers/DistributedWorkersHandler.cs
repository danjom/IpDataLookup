using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.API.Handlers
{
    public class DistributedWorkersHandler
    {

        public async Task<JObject> RetrieveDataFromWorkerAsync(string workerUri, string[] services)
        {
            JObject retrievedData = new JObject();

            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(workerUri);

                // Asynchronously get the JSON response.
                string result = response.Content.ReadAsStringAsync().Result;
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
