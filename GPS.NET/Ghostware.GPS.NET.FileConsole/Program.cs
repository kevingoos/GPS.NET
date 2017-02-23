using System;
using System.Runtime.InteropServices;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;

namespace Ghostware.GPS.NET.FileConsole
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

            var info = new FileGpsInfo
            {
                FilePath = "GPS.TXT",
                FileType = FileType.Nmea
            };
            _gpsService = new GpsService(info);

            _gpsService.RegisterDataEvent(GpsdServiceOnLocationChanged);
            _gpsService.Connect();

            Console.WriteLine("Press enter to continue...");
            Console.ReadKey();
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
