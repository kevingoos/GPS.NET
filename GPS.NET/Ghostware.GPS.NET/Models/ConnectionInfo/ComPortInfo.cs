namespace Ghostware.GPS.NET.Models.ConnectionInfo
{
    public class ComPortInfo : BaseGpsInfo
    {
        public string ComPort { get; set; }

        public ComPortInfo()
        {
            ReadFrequenty = 1000;
        }

        public ComPortInfo(string comPort, int readFrequenty = 1000)
        {
            ComPort = comPort;
            ReadFrequenty = readFrequenty;
        }
    }
}
