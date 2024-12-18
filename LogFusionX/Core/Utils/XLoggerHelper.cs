using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogFusionX.Core.Utils
{
    internal class XLoggerHelper
    {
        public XLoggerHelper()
        {
            
        }
        public string GetServerIpAddress()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return ip?.ToString() ?? "Server IP not found";
        }
    }
}
