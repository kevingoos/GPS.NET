using System.Runtime.InteropServices;
using System.Security;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.Events;

namespace Ghostware.GPS.NET.Console
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
            _gpsService.Connect(new GpsdData()
            {
                Address = "178.50.19.81",
                Port = 80,
                IsProxyEnabled = true,
                ProxyAddress = "proxy",
                ProxyPort = 80,
                IsProxyAuthManual = true,
                Username = "EXJ508",
                Password = "Xlssx531"
            });

            _gpsService.Client.GpsCallbackEvent += GpsdServiceOnLocationChanged;
        }

        private static void GpsdServiceOnLocationChanged(object sender, GpsDataEventArgs e)
        {
            System.Console.WriteLine(e.ToString());
        }

        private static bool ExitHandler()
        {
            _gpsService?.Disconnect();
            return true;
        }
    }
}
