using System;
using System.Device.Location;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;
using Ghostware.GPS.NET.Models.Events;

namespace Ghostware.GPS.NET.GpsClients
{
    public class WindowsLocationApiGpsClient : BaseGpsClient
    {
        #region Private Properties

        private GeoCoordinateWatcher _watcher;

        #endregion

        #region Constructors

        public WindowsLocationApiGpsClient() : base(GpsType.WindowsLocationApi)
        {

        }

        #endregion

        #region Connect and Disconnect

        public override bool Connect(IGpsInfo connectionData)
        {
            var data = (WindowsLocationApiInfo)connectionData;
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
