using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghostware.GPS.NET.Exceptions
{
    public class InvalidFileFormatException : Exception
    {
        public InvalidFileFormatException(string fileFormat, string latitudeHeader, string longitudeHeader) :
            base($"The file located at: {fileFormat} has an invalid file format! Please include a column named {latitudeHeader} and {longitudeHeader}.")
        {

        }
    }
}
