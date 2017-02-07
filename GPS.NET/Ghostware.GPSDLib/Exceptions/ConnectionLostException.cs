using System;

namespace Ghostware.GPSDLib.Exceptions
{
    public class ConnectionLostException : Exception
    {
        public ConnectionLostException() : base("The connection is lost.")
        {

        }
    }
}
