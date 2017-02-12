using System;
using System.IO;
using System.Threading;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;
using Ghostware.GPS.NET.Models.GpsdModels;
using Ghostware.GPS.NET.Parsers;
using Ghostware.NMEAParser;
using Ghostware.NMEAParser.Exceptions;
using Ghostware.NMEAParser.NMEAMessages;

namespace Ghostware.GPS.NET.GpsClients
{
    public class FileGpsClient : BaseGpsClient
    {
        #region Constructors

        public FileGpsClient(FileGpsInfo connectionData) : base(GpsType.File, connectionData)
        {
        }

        public FileGpsClient(BaseGpsInfo connectionData) : base(GpsType.File, connectionData)
        {
        }

        #endregion

        #region Connect and Disconnect

        public override bool Connect()
        {
            var data = (FileGpsInfo)GpsInfo;

            OnGpsStatusChanged(GpsStatus.Connecting);
            IsRunning = true;
            var parser = new NmeaParser();
            var gpsdDataParser = new GpsdDataParser();
            using (var streamReader = File.OpenText(data.FilePath))
            {
                string line;
                OnGpsStatusChanged(GpsStatus.Connected);
                while (IsRunning && (line = streamReader.ReadLine()) != null)
                {
                    try
                    {
                        OnRawGpsDataReceived(line);
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
                        Thread.Sleep(data.ReadFrequenty);
                    }
                    catch (UnknownTypeException)
                    {
                        Disconnect();
                        throw;
                    }
                }
                return true;
            }
        }

        public override bool Disconnect()
        {
            IsRunning = false;
            OnGpsStatusChanged(GpsStatus.Disabled);
            return true;
        }

        #endregion
    }
}