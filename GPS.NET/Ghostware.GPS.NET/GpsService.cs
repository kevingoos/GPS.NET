using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Exceptions;
using Ghostware.GPS.NET.Factories;
using Ghostware.GPS.NET.GpsClients.Interfaces;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET
{
    public class GpsService
    {
        public readonly BaseGpsClient Client;

        public GpsService(GpsType gpsServiceType)
        {
            Client = GpsClientFactory.Create(gpsServiceType);
        }

        public void Connect(IGpsData gpsData)
        {
            if (GpsDataFactory.GetDataType(Client.GpsType) != gpsData.GetType())
            {
                throw new InvalidDataTypeException();
            }

            Client.Connect(gpsData);
        }

        public void Disconnect()
        {
            Client.Disconnect();
        }
    }
}
