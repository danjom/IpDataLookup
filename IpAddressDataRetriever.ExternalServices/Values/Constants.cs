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
        public const string DNSLookup = "dnslookup";
        public const string ReverseDNSLookup = "reversednslookup";
        public const string DomainAvailability = "domainavailability";
        public const string GeoIp = "geoip";
        public const string IpAddress = "ipaddress";
        public const string Ping = "ping";
        public const string WhoIs = "whois";
        public const string RDAP = "rdap";
    }

}
