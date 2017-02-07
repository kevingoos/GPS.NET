using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Exceptions;
using Ghostware.GPS.NET.GpsClients.Interfaces;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;
using Ghostware.GPS.NET.Models.Events;
using Ghostware.GPSDLib;
using Ghostware.GPSDLib.Models;
using Ghostware.NMEAParser;
using Ghostware.NMEAParser.Exceptions;
using Ghostware.NMEAParser.NMEAMessages;

namespace Ghostware.GPS.NET.GpsClients
{
    public class FileGpsClient : BaseGpsClient
    {
        #region Constructors

        public FileGpsClient() : base(GpsType.File)
        {

        }

        #endregion

        #region Connect and Disconnect

        public override async Task<bool> Connect(IGpsData connectionData)
        {
            var data = (FileGpsData)connectionData;
            
            IsRunning = true;
            var parser = new NmeaParser();
            var gpsdDataParser = new GpsdDataParser();
            using (var streamReader = File.OpenText(data.FileLocation))
            {
                string line;
                while (IsRunning && (line = streamReader.ReadLine()) != null)
                {
                    try
                    {
                        var result = parser.Parse(line);
                        switch (data.FileType)
                        {
                            case FileType.Nmea:
                                OnGpsDataReceived(new GpsDataEventArgs((GprmcMessage)result));
                                break;
                            case FileType.Gpsd:
                                var message = gpsdDataParser.GetGpsData(line);
                                var gpsLocation = message as GpsLocation;
                                if (gpsLocation != null)
                                {
                                    OnGpsDataReceived(new GpsDataEventArgs(gpsLocation));
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        Thread.Sleep(data.TickTime);
                    }
                    catch (UnknownTypeException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Disconnect();
            }
        }

        public override async Task<bool> Disconnect()
        {
            IsRunning = false;
        }

        #endregion
    }
}