using Ghostware.GPS.NET.Models.GpsdModels;
using Newtonsoft.Json;

namespace Ghostware.GPS.NET.Parsers
{
    public class GpsdDataParser
    {
        public object GetGpsData(string gpsData)
        {
            try
            {
                var classType = JsonConvert.DeserializeObject<DataClassType>(gpsData);
                return JsonConvert.DeserializeObject(gpsData, classType.GetClassType());
            }
            catch (JsonReaderException)
            {
                return null;
            }
            catch (JsonSerializationException)
            {
                return null;
            }
        }
    }
}
