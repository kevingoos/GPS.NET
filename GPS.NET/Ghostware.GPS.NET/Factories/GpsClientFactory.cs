using System;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.GpsClients;
using Ghostware.GPS.NET.GpsClients.Interfaces;
using Ghostware.GPS.NET.Models;

namespace Ghostware.GPS.NET.Factories
{
    public static class GpsClientFactory
    {
        public static IBaseGpsClient Create(GpsType type)
        {
            switch (type)
            {
                case GpsType.File:
                    return new FileGpsClient();
                case GpsType.ComPort:
                    return new ComPortGpsClient();
                case GpsType.Gpsd:
                    return new GpsdGpsClient();
                case GpsType.WindowsLocationApi:
                    return new WindowsLocationApiGpsClient();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
