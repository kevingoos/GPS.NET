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
            _watcher = new GeoCoordinateWatcher();

            _watcher.PositionChanged += WatcherOnPositionChanged;
            _watcher.StatusChanged += WatcherOnStatusChanged;
            return _watcher.TryStart(false, TimeSpan.FromMilliseconds(data.Timeout));
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WatcherOnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            OnGpsDataReceived(new GpsDataEventArgs(e.Position.Location));
        }

        public override bool Disconnect()
        {
            _watcher.Stop();
            _watcher.Dispose();
            return true;
        }

        #endregion
    }
}
