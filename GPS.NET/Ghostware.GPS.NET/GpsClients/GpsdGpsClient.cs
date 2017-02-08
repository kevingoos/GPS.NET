using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ghostware.GPS.NET.Encryption;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.GpsClients.Interfaces;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;
using Ghostware.GPS.NET.Models.Events;
using Ghostware.GPSDLib;
using Ghostware.GPSDLib.Exceptions;
using Ghostware.GPSDLib.Models;

namespace Ghostware.GPS.NET.GpsClients
{
    public class GpsdGpsClient : BaseGpsClient
    {
        #region Private Properties

        private GpsdService _gpsdService;

        #endregion

        #region Constructors

        public GpsdGpsClient() : base(GpsType.Gpsd)
        {

        }

        #endregion

        #region Connect and Disconnect

        public override bool Connect(IGpsData connectionData)
        {
            var data = (GpsdData)connectionData;

            _gpsdService = new GpsdService(data.Address, data.Port);
            if (data.IsProxyEnabled)
            {
                _gpsdService.SetProxy(data.ProxyAddress, data.ProxyPort);
                if (data.IsProxyAuthManual)
                {
                    _gpsdService.SetProxyAuthentication(data.Username, data.Password);
                }
            }

            _gpsdService.OnLocationChanged += GpsdServiceOnOnLocationChanged;
            _gpsdService.OnRawLocationChanged += GpsdServiceOnOnRawLocationChanged;

            bool connected;
            try
            {
                connected = _gpsdService.Connect();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.FirstOrDefault()?.GetType() == typeof(WebException))
                {
                    return false;
                }
                throw;
            }

            if (connected)
            {
                _gpsdService.StartGpsReading();
            }
            return connected;
        }

        private void GpsdServiceOnOnRawLocationChanged(object source, string rawLocation)
        {
            OnRawGpsDataReceived(rawLocation);
        }

        private void GpsdServiceOnOnLocationChanged(object source, GpsLocation e)
        {
            OnGpsDataReceived(new GpsDataEventArgs(e));
        }

        public override bool Disconnect()
        {
            return _gpsdService.Disconnect();
        }

        #endregion
    }
}
