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
            return _watcher.TryStart(false, TimeSpan.FromMilliseconds(data.Timeout));
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
