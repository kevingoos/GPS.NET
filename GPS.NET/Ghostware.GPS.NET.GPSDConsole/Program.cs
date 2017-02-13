using System.Runtime.InteropServices;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;

namespace Ghostware.GPS.NET.GPSDConsole
{
    public class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler();
        private static EventHandler _eventHandler;

        private static GpsService _gpsService;

        static void Main(string[] args)
        {
            _eventHandler += ExitHandler;
            SetConsoleCtrlHandler(_eventHandler, true);

            var info = new GpsdInfo()
            {
                Address = "178.50.250.33",
                Port = 2947,
                IsProxyEnabled = true,
                ProxyAddress = "proxy",
                ProxyPort = 80,
                IsProxyAuthManual = true,
                ProxyUsername = "EXJ508",
                ProxyPassword = "Xlssx534"
            };
            _gpsService = new GpsService(info);

            _gpsService.RegisterDataEvent(GpsdServiceOnLocationChanged);
            _gpsService.Connect();

            System.Console.WriteLine("Press enter to continue...");
            System.Console.ReadKey();
        }

        private static void GpsdServiceOnLocationChanged(object sender, GpsDataEventArgs e)
        {
            System.Console.WriteLine(e.ToString());
        }

        private static bool ExitHandler()
        {
            return _gpsService == null || _gpsService.Disconnect();
        }
    }
}
