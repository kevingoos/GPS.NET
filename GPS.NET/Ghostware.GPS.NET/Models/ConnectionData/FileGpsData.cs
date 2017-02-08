using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.Models.ConnectionData
{
    public class FileGpsData : IGpsData
    {
        public string FileLocation { get; set; }
        public int TickTime { get; set; }
        public FileType FileType { get; set; }

        public FileGpsData(string fileLocation, FileType fileType = FileType.Nmea, int tickTime = 1)
        {
            FileLocation = fileLocation;
            FileType = fileType;
            TickTime = tickTime;
        }
    }
}
