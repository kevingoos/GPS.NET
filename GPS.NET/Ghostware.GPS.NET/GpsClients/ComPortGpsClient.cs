using System;
using System.IO.Ports;
using System.Threading;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;
using Ghostware.GPS.NET.Models.Events;
using Ghostware.NMEAParser;
using Ghostware.NMEAParser.Exceptions;
using Ghostware.NMEAParser.NMEAMessages;

namespace Ghostware.GPS.NET.GpsClients
{
    public class ComPortGpsClient : BaseGpsClient
    {
        #region Private Properties

        private readonly NmeaParser _parser = new NmeaParser();
        private SerialPort _serialPort;

        #endregion

        #region Constructors

        public ComPortGpsClient() : base(GpsType.ComPort)
        {
            
        }

        #endregion

        #region Connect and Disconnect

        public override bool Connect(IGpsData connectionData)
        {
            var data = (ComPortData)connectionData;

            _serialPort = new SerialPort(data.ComPort, 9600, Parity.None, 8, StopBits.One);

            // Attach a method to be called when there
            // is data waiting in the port's buffer
            _serialPort.DataReceived += port_DataReceived;

            // Begin communications
            _serialPort.Open();
            // Enter an application loop to keep this thread alive
            while (_serialPort.IsOpen)
            {
                Thread.Sleep(1000);
            }
            return true;
        }

        public override bool Disconnect()
        {
            _serialPort.Close();
            return true;
        }

        #endregion

        #region Location Callbacks

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var readString = _serialPort.ReadExisting();
                OnRawGpsDataReceived(readString);
                var result = _parser.Parse(readString);
                if (typeof(GprmcMessage) == result.GetType())
                {
                    OnGpsDataReceived(new GpsDataEventArgs((GprmcMessage)result));
                }
            }
            catch (UnknownTypeException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}