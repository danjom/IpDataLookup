﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.DataRetrivers.Abstraction
{
    /// <summary>
    /// This determines the met
    /// </summary>
    public interface IDataRetriever
    {
        Task<JObject> RetrieveDataAsync(string ipAddrOrDomainName, int inputType);
    }
}
