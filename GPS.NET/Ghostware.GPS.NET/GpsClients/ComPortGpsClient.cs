using System;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using crozone.SerialPorts.Abstractions;
using crozone.SerialPorts.LinuxSerialPort;
using crozone.SerialPorts.WindowsSerialPort;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;
using Ghostware.NMEAParser;
using Ghostware.NMEAParser.Exceptions;
using Ghostware.NMEAParser.NMEAMessages;
using Parity = crozone.SerialPorts.Abstractions.Parity;
using StopBits = crozone.SerialPorts.Abstractions.StopBits;

namespace Ghostware.GPS.NET.GpsClients
{
    public class ComPortGpsClient : BaseGpsClient
    {
        #region Private Properties

        private readonly NmeaParser _parser = new NmeaParser();
        private ISerialPort _serialPort;
        private StreamReader _reader;

        private DateTime? _previousReadTime;

        #endregion

        #region Constructors

        public ComPortGpsClient(ComPortInfo connectionData) : base(GpsType.ComPort, connectionData)
        {
        }

        public ComPortGpsClient(BaseGpsInfo connectionData) : base(GpsType.ComPort, connectionData)
        {
        }

        #endregion

        #region Connect and Disconnect

        public override bool Connect()
        {
            var data = (ComPortInfo)GpsInfo;

            IsRunning = true;
            OnGpsStatusChanged(GpsStatus.Connecting);

            _serialPort = CreatePort(data.ComPort);
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;

            try
            {
                // Begin communications
                _serialPort.Open();
                _reader = new StreamReader(_serialPort.BaseStream, Encoding.ASCII);

                OnGpsStatusChanged(GpsStatus.Connected);
                // Enter an application loop to keep this thread alive
                while (_serialPort.IsOpen)
                {
                    ReadLine();
                    Thread.Sleep(data.ReadFrequenty);
                }
            }
            catch
            {
                Disconnect();
                throw;
            }
            
            return true;
        }

        public override bool Disconnect()
        {
            _serialPort.Close();
            IsRunning = false;
            OnGpsStatusChanged(GpsStatus.Disabled);
            return true;
        }

        private static ISerialPort CreatePort(string portName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return new LinuxSerialPort(portName);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return new WindowsSerialPort(new SerialPort(portName));
            else throw new InvalidOperationException("unsupported operating system");
        }

        #endregion

        #region Location Callbacks

        private void ReadLine()
        {
            try
            {
                var readString = _reader.ReadLine();
                OnRawGpsDataReceived(readString);
                var result = _parser.Parse(readString);
                if (typeof(GprmcMessage) != result.GetType()) return;
                if (_previousReadTime != null && GpsInfo.ReadFrequenty != 0 && ((GprmcMessage)result).UpdateDate.Subtract(new TimeSpan(0, 0, 0, 0, GpsInfo.ReadFrequenty)) <= _previousReadTime) return;
                OnGpsDataReceived(new GpsDataEventArgs((GprmcMessage)result));
            }
            catch (UnknownTypeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}