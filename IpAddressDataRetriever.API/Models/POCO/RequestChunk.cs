using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.API.Models.POCO
{
    public class RequestChunk
    {
        public List<string> Services { set; get; }
        public bool MasterAssigned { set; get; }
    }
}
