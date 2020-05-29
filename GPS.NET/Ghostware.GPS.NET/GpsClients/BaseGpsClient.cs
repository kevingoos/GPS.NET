using System;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;

namespace Ghostware.GPS.NET.GpsClients
{
    public abstract class BaseGpsClient
    {
        #region Properties

        public GpsType GpsType { get; }
        public bool IsRunning { get; set; }

        protected BaseGpsInfo GpsInfo { get; set; }

        #endregion

        #region Event handlers

        public event EventHandler<GpsDataEventArgs> GpsCallbackEvent;
        public event EventHandler<string> RawGpsCallbackEvent;
        public event EventHandler<GpsStatus> GpsStatusEvent;

        #endregion

        #region Constructors

        protected BaseGpsClient(GpsType gpsType, BaseGpsInfo gpsInfo)
        {
            GpsType = gpsType;
            GpsInfo = gpsInfo;
        }

        #endregion

        #region Connect and Disconnect

        public abstract bool Connect();

        public abstract bool Disconnect();

        #endregion

        #region Events Triggers

        protected virtual void OnGpsDataReceived(GpsDataEventArgs e)
        {
            if (GpsInfo.CoordinateSystem != GpsCoordinateSystem.GeoEtrs89)
            {
                throw new InvalidOperationException(nameof(GpsCoordinateSystem) + "." + nameof(GpsCoordinateSystem.GeoEtrs89) + " expected");
            }

            GpsCallbackEvent?.Invoke(this, e);
        }

        protected virtual void OnRawGpsDataReceived(string e)
        {
            RawGpsCallbackEvent?.Invoke(this, e);
        }

        protected virtual void OnGpsStatusChanged(GpsStatus e)
        {
            GpsStatusEvent?.Invoke(this, e);
        }

        #endregion
    }
}