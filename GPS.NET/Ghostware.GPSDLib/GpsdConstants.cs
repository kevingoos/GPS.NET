using Ghostware.GPSDLib.Models;

namespace Ghostware.GPSDLib
{
    public static class GpsdConstants
    {
        public static GpsdOptions DefaultGpsdOptions = new GpsdOptions {Enable = true, Json = true};
        public const string DisableCommand = "?WATCH={\"enable\":false}";
        public const string PollCommand = "?POLL;";
    }
}
