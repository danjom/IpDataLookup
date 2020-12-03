using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.Values
{
    public static class InputTypes
    {
        public const int Invalid = 0;
        public const int IpAddressv4 = 1;
        public const int IpAddressv6 = 2;
        public const int DomainName = 3;
    }

    public static class DataServices
    {
        public const string DNSLookup = "DNSLookup";
        public const string DomainAvailability = "DomainAvailability";
        public const string GeoIp = "GeoIp";
        public const string IpAddress = "IpAddress";
        public const string Ping = "Ping";
        public const string ReverseDNSLookup = "ReverseDNSLookup";
        public const string WhoIs = "WhoIs";
    }

}
