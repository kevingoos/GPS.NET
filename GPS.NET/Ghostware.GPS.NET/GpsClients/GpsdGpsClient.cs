using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.GpsClients.Interfaces;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.GpsClients
{
    public class GpsdGpsClient : BaseGpsClient
    {
        #region Constructors

        public GpsdGpsClient() : base(GpsType.Gpsd)
        {

        }

        #endregion

        #region Connect and Disconnect

        public override void Connect(IGpsData connectionData)
        {
            throw new System.NotImplementedException();
        }

        public override void Disconnect()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
