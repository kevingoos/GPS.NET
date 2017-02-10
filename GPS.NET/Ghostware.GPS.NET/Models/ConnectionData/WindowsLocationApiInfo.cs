using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.Models.ConnectionData
{
    public class WindowsLocationApiInfo : IGpsInfo
    {
        public int Timeout { get; set; } = 1000;
    }
}
