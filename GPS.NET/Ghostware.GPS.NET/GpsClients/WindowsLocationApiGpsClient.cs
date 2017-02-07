using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.GpsClients.Interfaces;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.GpsClients
{
    public class WindowsLocationApiGpsClient : IBaseGpsClient
    {
        public GpsType GpsType => GpsType.WindowsLocationApi;

        public void Connect(IGpsData connectionData)
        {
            throw new System.NotImplementedException();
        }

        public void Disconnect()
        {
            throw new System.NotImplementedException();
        }
    }
}
