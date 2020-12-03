using IpAddressDataRetriever.API.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.API.Handlers
{
    public class LoadBalancer
    {
        public List<RequestChunk> SplitServices(List<string> services, int workersCount, int masterIncluded)
        {
            List<RequestChunk> requestChunks = new List<RequestChunk>();

            int chunkSize = services.Count / workersCount;
            int remainder = services.Count % workersCount;

            if(chunkSize > 0)
            {

            }

            if(remainder > 0)
            {

            }

            return requestChunks;
        }
    }
}
