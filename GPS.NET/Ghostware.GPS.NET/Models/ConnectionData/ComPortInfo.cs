using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.Models.ConnectionData
{
    public class ComPortInfo : IGpsInfo
    {
        public string ComPort { get; set; }
        public int ReadFrequenty { get; set; } = 1000;

        public ComPortInfo()
        {
            
        }

        public ComPortInfo(string comPort, int readFrequenty = 1000)
        {
            ComPort = comPort;
            ReadFrequenty = readFrequenty;
        }
    }
}
