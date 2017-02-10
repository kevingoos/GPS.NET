using System.Runtime.InteropServices;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData;
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

            _gpsService = new GpsService(GpsType.Gpsd);

            _gpsService.RegisterDataEvent(GpsdServiceOnLocationChanged);
            _gpsService.Connect(new GpsdInfo()
            {
                Address = "***.* **.* **.***",
                //Default
                //Port = 2947,
                //IsProxyEnabled = true,
                //ProxyAddress = "proxy",
                //ProxyPort = 80,
                //IsProxyAuthManual = true,
                //ProxyUsername = "*****",
                //ProxyPassword = "*****"
            });

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
