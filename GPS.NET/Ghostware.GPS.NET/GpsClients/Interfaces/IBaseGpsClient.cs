using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.GpsClients.Interfaces
{
    public interface IBaseGpsClient
    {
        GpsType GpsType { get; }

        void Connect(IGpsData connectionData);

        void Disconnect();
    }
}
