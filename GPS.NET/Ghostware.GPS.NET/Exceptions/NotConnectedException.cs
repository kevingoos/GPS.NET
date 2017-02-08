using System;

namespace Ghostware.GPSDLib.Exceptions
{
    public class NotConnectedException : Exception
    {
        public NotConnectedException() : base("The connection is not open. Plz connect first!")
        {

        }
    }
}
