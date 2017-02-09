using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.Models.ConnectionData
{
    public class FileGpsInfo : IGpsInfo
    {
        public string FileLocation { get; set; }
        public FileType FileType { get; set; }

        public int ReadFrequenty { get; set; } = 1000;

        public FileGpsInfo()
        {
            
        }

        public FileGpsInfo(string fileLocation, FileType fileType = FileType.Nmea, int readFrequenty = 1000)
        {
            FileLocation = fileLocation;
            FileType = fileType;
            ReadFrequenty = readFrequenty;
        }
    }
}
