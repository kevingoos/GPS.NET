namespace Ghostware.GPS.NET.Models.ConnectionInfo
{
    public class ComPortInfo : BaseGpsInfo
    {
        public string ComPort { get; set; } = "ComPort1";
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
