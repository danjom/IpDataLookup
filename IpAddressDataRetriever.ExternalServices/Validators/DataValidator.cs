using IpAddressDataRetriever.Services.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IpAddressDataRetriever.Services.Validators
{
    public static class DataValidator
    {
        public static int GetInputType(string input)
        {
            int inputType = InputTypes.Invalid;

            if (Regex.Match(input, @"([a-zA-Z0-9\-]{1,63}\.)*[a-zA-Z\-]{1,63}", RegexOptions.IgnoreCase).Success) inputType = InputTypes.DomainName;

            if (inputType == InputTypes.Invalid)
            {
                //First will check if it's an IP
                if (!IPAddress.TryParse(input, out IPAddress ip)) inputType = InputTypes.Invalid;

                switch (ip.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        if (input.Length > 6 && input.Contains("."))
                        {
                            string[] s = input.Split('.');
                            if (s.Length == 4 && s[0].Length > 0 && s[1].Length > 0 && s[2].Length > 0 && s[3].Length > 0)
                                inputType = InputTypes.IpAddressv4;
                        }
                        break;
                    case AddressFamily.InterNetworkV6:
                        if (input.Contains(":") && input.Length > 15)
                            inputType = InputTypes.IpAddressv6;
                        break;
                }

            }

            return inputType;
        }
    }
}
