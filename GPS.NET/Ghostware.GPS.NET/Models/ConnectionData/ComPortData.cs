using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.Models.ConnectionData
{
    public struct ComPortData : IGpsData
    {
        public string ComPort { get; set; }
    }
}
