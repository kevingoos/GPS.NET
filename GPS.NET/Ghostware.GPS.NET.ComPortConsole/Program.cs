using System;
using System.Runtime.InteropServices;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;

namespace Ghostware.GPS.NET.ComPortConsole
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

            var info = new ComPortInfo()
            {
                ComPort = "COM9\0\0\0\0",
            };
            _gpsService = new GpsService(info);
            _gpsService.RegisterStatusEvent(Action);
            _gpsService.RegisterDataEvent(GpsdServiceOnLocationChanged);

            try
            {
                _gpsService.Connect();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("The selected com port is already in use!");
            }

            Console.WriteLine("Press enter to continue...");
            Console.ReadKey();
        }

        private static void Action(object o, GpsStatus gpsStatus)
        {
            Console.WriteLine(gpsStatus);
        }

        private static void GpsdServiceOnLocationChanged(object sender, GpsDataEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }

        private static bool ExitHandler()
        {
            return _gpsService == null || _gpsService.Disconnect();
        }
    }
}
