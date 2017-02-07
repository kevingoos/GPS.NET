using System.Threading.Tasks;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.GpsClients.Interfaces;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.ConnectionData.Interfaces;

namespace Ghostware.GPS.NET.GpsClients
{
    public class WindowsLocationApiGpsClient : BaseGpsClient
    {
        #region Constructors

        public WindowsLocationApiGpsClient() : base(GpsType.WindowsLocationApi)
        {

        }

        #endregion

        #region Connect and Disconnect

        public override async Task<bool> Connect(IGpsData connectionData)
        {
            var data = (WindowsLocationApiData)connectionData;
        }

        public override async Task<bool> Disconnect()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
