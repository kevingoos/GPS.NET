using System;
using System.Device.Location;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;

namespace Ghostware.GPS.NET.GpsClients
{
    public class WindowsLocationApiGpsClient : BaseGpsClient
    {
        #region Private Properties

        private GeoCoordinateWatcher _watcher;

        private DateTimeOffset? _previousReadTime;

        #endregion

        #region Constructors

        public WindowsLocationApiGpsClient(WindowsLocationApiInfo connectionData) : base(GpsType.WindowsLocationApi, connectionData)
        {
        }

        public WindowsLocationApiGpsClient(BaseGpsInfo connectionData) : base(GpsType.WindowsLocationApi, connectionData)
        {
        }

        #endregion

        #region Connect and Disconnect

        public override bool Connect()
        {
            var data = (WindowsLocationApiInfo)GpsInfo;

            IsRunning = true;
            OnGpsStatusChanged(GpsStatus.Connecting);

            _watcher = new GeoCoordinateWatcher();
            
            _watcher.PositionChanged += WatcherOnPositionChanged;
            _watcher.StatusChanged += WatcherOnStatusChanged;

            bool result;
            try
            {
                result = _watcher.TryStart(false, TimeSpan.FromMilliseconds(data.Timeout));
            }
            catch
            {
                Disconnect();
                throw;
            }
            return result;
        }

        private void WatcherOnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Ready:
                    OnGpsStatusChanged(GpsStatus.Connected);
                    break;
                case GeoPositionStatus.Initializing:
                    OnGpsStatusChanged(GpsStatus.Connecting);
                    break;
                case GeoPositionStatus.NoData:
                    OnGpsStatusChanged(GpsStatus.Connecting);
                    break;
                case GeoPositionStatus.Disabled:
                    OnGpsStatusChanged(GpsStatus.Disabled);
                    break;
            }
        }

        private void WatcherOnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (_previousReadTime != null && GpsInfo.ReadFrequenty != 0 && e.Position.Timestamp.Subtract(new TimeSpan(0, 0, 0, 0, GpsInfo.ReadFrequenty)) <= _previousReadTime) return;
            OnGpsDataReceived(new GpsDataEventArgs(e.Position.Location));
            _previousReadTime = e.Position.Timestamp;
        }

        public override bool Disconnect()
        {
            IsRunning = false;
            _watcher.Stop();
            _watcher.Dispose();
            OnGpsStatusChanged(GpsStatus.Disabled);
            return true;
        }

        #endregion
    }
}
