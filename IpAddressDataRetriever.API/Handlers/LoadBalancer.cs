using IpAddressDataRetriever.API.Models.POCO;
using System.Collections.Generic;

namespace IpAddressDataRetriever.API.Handlers
{
    public static class LoadBalancer
    {
        public static List<RequestChunk> SplitServices(List<string> services, int workersCount, bool masterIncluded)
        {
            List<RequestChunk> requestChunks = new List<RequestChunk>();

            int chunkSize = services.Count / workersCount;
            int remainder = services.Count % workersCount;
            int index = 0; 

            if(chunkSize > 0)
            {
                for(int i = 0; i < workersCount; ++i)
                {
                    requestChunks.Add(new RequestChunk
                    {
                        MasterAssigned = masterIncluded && i == workersCount - 1,
                        Services = services.GetRange(index, chunkSize)
                    });

                    index += chunkSize;
                }
            }

            if(remainder > 0)
            {
                for(int i = 0; i < remainder; ++i)
                {
                    requestChunks[i].Services.Add(services[index++]);
                }
            }

            return requestChunks;
        }
    }
}
