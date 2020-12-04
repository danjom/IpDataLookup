using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.Models.POCO
{
    public class ResponseData
    {
        public System.Net.HttpStatusCode StatusCode { set; get; }
        public string ResponseBody { set; get; }
    }
}
