using System;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.Factories
{
    public static class GpsDataFactory
    {
        public static IGpsData Create(GpsType type)
        {
            var dataType = GetDataType(type);
            return (IGpsData)Activator.CreateInstance(dataType);
        }

        public static Type GetDataType(GpsType type)
        {
            switch (type)
            {
                case GpsType.File:
                    return typeof(FileGpsData);
                case GpsType.ComPort:
                    return typeof(ComPortData);
                case GpsType.Gpsd:
                    return typeof(GpsdData);
                case GpsType.WindowsLocationApi:
                    return typeof(WindowsLocationApiData);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
