using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Ghostware.GPSDLib;
using Ghostware.GPSDLib.Models;

namespace Ghostware.GPSDTestConsole
{
    public class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler();
        private static EventHandler _eventHandler;

        private static StreamWriter _writer;

        private static GpsdService _gpsdService;

        static void Main(string[] args)
        {
            _eventHandler += ExitHandler;
            SetConsoleCtrlHandler(_eventHandler, true);

            _gpsdService = new GpsdService("***.***.***.***", 80);
            //_gpsdService = new GpsdService("127.0.0.1", 80);

            //_gpsdService.SetProxy("proxy", 80);
            //_gpsdService.SetProxyAuthentication("*****", "*****");

            _writer = new StreamWriter("testFile1.nmea");

            _gpsdService.OnRawLocationChanged += GpsdServiceOnRawLocationChanged;
            _gpsdService.OnLocationChanged += GpsdServiceOnLocationChanged;

            try
            {
                Retry.Do(_gpsdService.Connect, TimeSpan.FromSeconds(1));
            }
            catch (AggregateException ax)
            {
                if (ax.InnerExceptions.Any(x => x.GetType() == typeof(WebException)))
                {
                    Console.WriteLine("Cannot connect to the service you have given.");
                }
            }

            _gpsdService.StartGpsReading();

            Console.WriteLine("Press enter to continue...");
            Console.ReadKey();
        }

        private static void GpsdServiceOnLocationChanged(object source, GpsLocation e)
        {
            Console.WriteLine(e.ToString());
        }

        private static void GpsdServiceOnRawLocationChanged(object source, string rawData)
        {
            _writer.WriteLine(rawData);
        }

        private static bool ExitHandler()
        {
            _gpsdService?.Disconnect();
            return true;
        }
    }
}
