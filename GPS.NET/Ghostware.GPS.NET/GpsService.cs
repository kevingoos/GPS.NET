using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Factories;
using Ghostware.GPS.NET.GpsClients.Interfaces;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET
{
    public class GpsService
    {
        private readonly IBaseGpsClient _client;

        public GpsService(GpsType gpsServiceType)
        {
            _client = GpsClientFactory.Create(gpsServiceType);
        }

        public void Connect(IGpsData gpsData)
        {


            _client.Connect(gpsData);
        }
    }
}
