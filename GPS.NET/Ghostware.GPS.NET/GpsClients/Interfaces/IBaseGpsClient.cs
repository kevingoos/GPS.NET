using System;
using System.Threading.Tasks;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;
using Ghostware.GPS.NET.Models.Events;

namespace Ghostware.GPS.NET.GpsClients.Interfaces
{
    public abstract class BaseGpsClient
    {
        #region Properties

        public GpsType GpsType { get; }
        public bool IsRunning { get; set; }

        #endregion
        
        #region Event handlers

        public event EventHandler<GpsDataEventArgs> GpsCallbackEvent;

        #endregion

        #region Constructors

        protected BaseGpsClient(GpsType gpsType)
        {
            GpsType = gpsType;
        }

        #endregion

        #region Connect and Disconnect

        public abstract Task<bool> Connect(IGpsData connectionData);

        public abstract Task<bool> Disconnect();

        #endregion

        #region Events Triggers

        protected virtual void OnGpsDataReceived(GpsDataEventArgs e)
        {
            var handler = GpsCallbackEvent;
            handler?.Invoke(this, e);
        }

        #endregion
    }
}