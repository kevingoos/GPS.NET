using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.Models.ConnectionData
{
    public struct GpsdData : IGpsData
    {
        public string Address { get; set; }
        public int Port { get; set; }

        public bool IsProxyEnabled { get; set; }

        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }

        public bool IsProxyAuthManual { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public byte[] EncryptionKey { get; set; }
    }
}
