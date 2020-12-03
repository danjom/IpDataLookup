using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Abstraction
{
    public abstract class APIDataRetriever : IDataRetriever
    {
        private readonly string endpointUrl;


        public static async Task<string> ApiRetrieverAsync(string endpointUri)
        {
            string result = "";

            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(endpointUri);

                // Asynchronously get the JSON response.
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

            return result;
        }

        public abstract Task<JObject> RetrieveDataAsync(string address);
    }
}
