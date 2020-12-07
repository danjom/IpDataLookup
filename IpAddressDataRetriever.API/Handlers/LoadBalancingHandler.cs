using IpAddressDataRetriever.API.Models.POCO;
using System.Collections.Generic;

namespace IpAddressDataRetriever.API.Handlers
{
    public static class LoadBalancingHandler
    {

        /// <summary>
        /// This method splits the services to query in as much chunks as @workersCount
        /// In case master will be considered a workers, considering that besides that
        /// master needs to merge all the responses and handle the communication with the other workers
        /// will be the one with less tasks to complete in case can't be splitted evenly between all workers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="workersCount"></param>
        /// <param name="masterIncluded"></param>
        /// <returns></returns>
        public static List<RequestChunk> SplitServices(List<string> services, int workersCount, bool masterIncluded)
        {
            List<RequestChunk> requestChunks = new List<RequestChunk>();

            if(workersCount > 0 && services?.Count > 0)
            {

                int chunkSize = services.Count / workersCount;
                int remainder = services.Count % workersCount;
                int index = 0;

                if (chunkSize > 0)
                {
                    for (int i = 0; i < workersCount; ++i)
                    {
                        requestChunks.Add(new RequestChunk
                        {
                            MasterAssigned = masterIncluded && i == workersCount - 1,
                            Services = services.GetRange(index, chunkSize)
                        });

                        index += chunkSize;
                    }
                }

                if (remainder > 0)
                {
                    for (int i = 0; i < remainder; ++i)
                    {
                        if (requestChunks.Count > i)
                        {
                            requestChunks[i].Services.Add(services[index++]);
                        }
                        else
                        {
                            requestChunks.Add(new RequestChunk
                            {
                                MasterAssigned = masterIncluded && i == workersCount - 1,
                                Services = services.GetRange(index++, 1)
                            });
                        }

                    }
                }
            }

            return requestChunks;
        }
    }
}
