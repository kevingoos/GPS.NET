using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Ghostware.GPSDLib.Exceptions;

namespace Ghostware.GPSDLib.Models
{
    [DataContract]
    public class DataClassType
    {
        public static Dictionary<string, Type> TypeDictionary = new Dictionary<string, Type>
        {
            {"VERSION", typeof(GpsdVersion)},
            {"DEVICES", typeof(GpsDevices)},
            {"WATCH", typeof(GpsdOptions)},
            {"TPV", typeof(GpsLocation)}
        };

        [DataMember(Name = "class")]
        public string Class { get; set; }

        public Type GetClassType()
        {
            Type result;
            TypeDictionary.TryGetValue(Class, out result);

            if (result == null)
            {
                throw new UnknownTypeException();
            }

            return result;
        }
    }
}
