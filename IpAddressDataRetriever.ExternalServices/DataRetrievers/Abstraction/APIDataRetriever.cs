using IpAddressDataRetriever.Services.Models.POCO;
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

        public static async Task<ResponseData> ApiRetrieverAsync(string endpointUri)
        {
            ResponseData result = new ResponseData();

            try
            {
                if (!string.IsNullOrWhiteSpace(endpointUri))
                {

                    HttpClient client = new HttpClient();

                    HttpResponseMessage response = await client.GetAsync(endpointUri);

                    if (response != null)
                    {
                        result.StatusCode = response.StatusCode;
                        // Asynchronously get the JSON response.
                        result.ResponseBody = response.Content.ReadAsStringAsync().Result;
                    }
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }

            return result;
        }

        public abstract Task<JObject> RetrieveDataAsync(string address, int inputType);
    }
}
