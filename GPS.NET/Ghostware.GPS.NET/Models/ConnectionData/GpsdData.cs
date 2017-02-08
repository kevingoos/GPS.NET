using Ghostware.GPS.NET.Constants;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;
using Ghostware.GPS.NET.Models.GpsdModels;

namespace Ghostware.GPS.NET.Models.ConnectionData
{
    public class GpsdData : IGpsData
    {
        public string Address { get; set; }
        public int Port { get; set; }

        public bool IsProxyEnabled { get; set; }

        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }

        public bool IsProxyAuthManual { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }

        public int ReadFrequenty { get; set; } = 1000;
        public int RetryRead { get; set; } = 3;

        public GpsdOptions GpsOptions { get; set; } = GpsdConstants.DefaultGpsdOptions;
    }
}
