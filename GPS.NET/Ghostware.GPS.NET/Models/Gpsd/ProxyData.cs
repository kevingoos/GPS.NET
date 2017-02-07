using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostware.GPS.NET.Models.Gpsd
{
    public class ProxyData
    {
        public string Address { get; set; }
        public int Port { get; set; }

        public bool IsProxyAuthManual { get; set; }
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }
    }
}
