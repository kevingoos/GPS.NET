using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghostware.GPSDLib.Models;
using Ghostware.NMEAParser.NMEAMessages;

namespace Ghostware.GPS.NET.Models.Events
{
    public class GpsDataEventArgs : EventArgs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Speed { get; set; }

        public double Measure { get; set; }
        public bool IsMeasureCalculated { get; set; }

        public string RawData { get; set; }

        public GpsDataEventArgs(GpsLocation gpsLocation)
        {
            Latitude = gpsLocation.Latitude;
            Longitude = gpsLocation.Longitude;
            Speed = gpsLocation.Speed;
        }

        public GpsDataEventArgs(GprmcMessage gpsLocation)
        {
            Latitude = gpsLocation.Latitude;
            Longitude = gpsLocation.Longitude;
            Speed = gpsLocation.Speed;
        }
    }
}
