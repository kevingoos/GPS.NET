using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Exceptions;
using Ghostware.GPS.NET.Factories;
using Ghostware.GPS.NET.GpsClients.Interfaces;
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
            if (GpsDataFactory.GetDataType(_client.GpsType) != gpsData.GetType())
            {
                throw new InvalidDataTypeException();
            }

            _client.Connect(gpsData);
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }
    }
}
