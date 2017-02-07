using Ghostware.GPS.NET.Models;

namespace Ghostware.GPS.NET
{
    public class GpsService
    {
        private GpsServiceType _gpsServiceType;

        public GpsService(GpsServiceType gpsServiceType)
        {
            _gpsServiceType = gpsServiceType;
        }

    }
}
